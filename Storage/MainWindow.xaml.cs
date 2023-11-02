using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Media;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Storage
{
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        SqlCommand command;
        private DataTable table;
        SqlDataAdapter adapter;

        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            command = new SqlCommand();
            command.Connection = conn;
        }

        private void SetConnectionIndicatorColor(Brush color)
        {
            ConnectionIndicator.Fill = color;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                SetConnectionIndicatorColor(Brushes.Green);
                MessageBox.Show("Connection successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
                SetConnectionIndicatorColor(Brushes.Red);
                MessageBox.Show("Disconnected from the database.");
            }
        }

        private void Show_All_Products_Click(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(ConfigurationManager.AppSettings[0], conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }

        private void Show_All_Type_Click(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(ConfigurationManager.AppSettings[1], conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }

        private void Show_All_Delivery_Click(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(ConfigurationManager.AppSettings[2], conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }

        private void Show_Products_Max_Count_Click(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(ConfigurationManager.AppSettings[3], conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }

        private void Show_Products_Min_Count_Click(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(ConfigurationManager.AppSettings[4], conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }

        private void Show_Products_Min_Price_Click(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(ConfigurationManager.AppSettings[5], conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }

        private void Show_Products_Max_Price_Click(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(ConfigurationManager.AppSettings[6], conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }

        private void Show_Product_Given_Type(object sender, RoutedEventArgs e)
        {
            if (TextBox.Text != string.Empty)
            {             
                string str = "Select * from Products where  TypePrductId IN (SELECT Id FROM TypeProducts WHERE Name = @typeName)";
                command = new SqlCommand();
                command.CommandText = str;
                command.Parameters.AddWithValue("@typeName", TextBox.Text);
                command.Connection = conn;

                table = new DataTable();
                adapter = new SqlDataAdapter(command);

                adapter.Fill(table);
                DataGrid.ItemsSource = table.DefaultView;
            }
        }

        private void Show_Product_Given_Delivery(object sender, RoutedEventArgs e)
        {
            if (TextBox.Text != string.Empty)
            {
                string str = "Select P.Name from dbo.Products as P join TypeProducts as TP on P.TypePrductid = TP.id where P.Name = @typeName";
                command = new SqlCommand();
                command.CommandText = str;
                command.Parameters.AddWithValue("@typeName", TextBox.Text);
                command.Connection = conn;

                table = new DataTable();
                adapter = new SqlDataAdapter(command);

                adapter.Fill(table);
                DataGrid.ItemsSource = table.DefaultView;
            }
        }

        string str1 = "SELECT TOP 1 * FROM Products ORDER BY DateOfDelivery ASC;"; //старый товар  
        private void Show_Prod_old(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(str1, conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }

        string str2 = "SELECT TP.Name, AVG(P.Count) as 'Count' FROM Products as P JOIN TypeProducts as TP ON P.TypePrductId = TP.Id GROUP BY TP.Name;"; //avg  товара по каждому типу
        private void Show_Prod_avg_type(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            adapter = new SqlDataAdapter(str2, conn);
            adapter.Fill(table);
            DataGrid.ItemsSource = table.DefaultView;
        }
    }
}
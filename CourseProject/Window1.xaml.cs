using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;

namespace CourseProject
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        public void DataRefresh()
        {
            MainDataGrid.Items.Refresh();
        }

        private void ButtonCloseMain(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void CmdMinimizeMain_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void AddOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddOrder add = new AddOrder();
            add.ShowDialog();

        }

        private void AddClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Main.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Visible;
            Work.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Collapsed;
            string dbcon = "Data Source = AddOrder.db; Version = 3";
            string query = "SELECT Name,Phone FROM Clients";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = select_command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    ClientsDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            string dbcon = "Data Source = AddOrder.db; Version = 3";
            string query = "SELECT Name,Phone FROM Clients";
            
            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = select_command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    MainDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void Masters_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Visible;
            Work.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Collapsed;
            string dbcon = "Data Source = AddOrder.db; Version = 3";
            string query = "SELECT Name,Phone,IDNumber FROM Masters";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = select_command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    MasterDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void AddClientClick(object sender, RoutedEventArgs e)
        {
            AddClient add = new AddClient();
            add.ShowDialog();
        }

        private void AddMasterClick(object sender, RoutedEventArgs e)
        {
            AddMaster add = new AddMaster();
            add.ShowDialog();
        }

        private void AddDeviceClick(object sender, RoutedEventArgs e)
        {
            AddDevice add = new AddDevice();
            add.ShowDialog();
        }

        private void AddWorkClick(object sender, RoutedEventArgs e)
        {
            AddWork add = new AddWork();
            add.ShowDialog();
        }

        private void Works_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Work.Visibility = Visibility.Visible;
            Main.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Collapsed;
            string dbcon = "Data Source = AddOrder.db; Version = 3";
            string query = "SELECT Works.Name, Works.Price, Works.DeviceID, Devices.id, Devices.Model FROM Works, Devices WHERE DeviceID = Devices.id;";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = select_command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    WorksDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void Devices_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Visible;
            Work.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Collapsed;
            string dbcon = "Data Source = AddOrder.db; Version = 3";
            //string query = "SELECT Тип,Бренд,Модель FROM Devices";
            string query = "SELECT Types.Name, Brands.BrandName, Devices.Model From Brands INNER JOIN(Types INNER JOIN Devices ON Types.id = Devices.TypeId) ON Brands.id = Devices.BrandId";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = select_command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    DevicesDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void Orders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Main.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Collapsed;
            Work.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Visible;
            string dbcon = "Data Source = AddOrder.db; Version = 3";
            string query = "SELECT * FROM Application";
            //string query = "SELECT Application.";
            SQLiteConnection connection = new SQLiteConnection(dbcon);
            connection.Open();
            SQLiteCommand select_command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = select_command.ExecuteReader();
            DataTable dt = new DataTable(); //Создаем новый DataTable
            dt.Load(reader); //Загружаем DataReader в DataTable
            OrdersDataGrid.ItemsSource = dt.DefaultView;
            reader.Close();
            connection.Close();
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            //MasterDataGrid.Items.Refresh();
            /*string dbcon_1 = "Data Source = AddOrder.db; Version=3;";
            string query_search_1 = "SELECT Name,Phone,IDNumber FROM Masters";
            SQLiteConnection connection_1 = new SQLiteConnection(dbcon_1);
            connection_1.Open();
            SQLiteCommand command_search_1 = new SQLiteCommand(query_search_1, connection_1);
            SQLiteDataReader reader = command_search_1.ExecuteReader();
            DataTable DT = new DataTable();
            DT.Load(reader);
            MasterDataGrid.ItemsSource = DT.DefaultView;
            reader.Close();
            connection_1.Close();*/
        }

        private void ClientsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if(grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                DataGridRow dataGridRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                DataRowView rowView = (DataRowView)dataGridRow.Item;

                string ClientName = rowView[0].ToString();
                string ClientPhone = rowView[1].ToString();
                Window_Loaded window = new Window_Loaded();
                window.FullName.Text = ClientName;
                window.PhoneNumber.Text = ClientPhone;
                window.ShowDialog();
            }
        }

        private void MasterDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if(grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                DataGridRow dataGridRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                DataRowView rowView = (DataRowView)dataGridRow.Item;

                string MasterName = rowView[0].ToString();
                string MasterPhone = rowView[1].ToString();
                ShowAndEditMaster window = new ShowAndEditMaster();
                window.FullName.Text = MasterName;
                window.PhoneNumber.Text = MasterPhone;
                window.ShowDialog();
            }
        }

        private void WorksDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                DataGridRow dataGridRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                DataRowView rowView = (DataRowView)dataGridRow.Item;

                string WorkName = rowView[0].ToString();
                string WorkPrice = rowView[1].ToString();
                string DeviceModel = rowView[2].ToString();
                int DeviceID = int.Parse(DeviceModel);
                ShowAndEditWork window = new ShowAndEditWork();

                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                string SelectModel = "SELECT Model FROM Devices WHERE id = @DeviceID";

                using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                {
                    connection.Open();
                    SQLiteCommand SelectModel_command = new SQLiteCommand(SelectModel, connection);
                    SelectModel_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceID;

                    using (SQLiteDataReader reader = SelectModel_command.ExecuteReader())
                    {
                        reader.Read();
                        string Model = reader.GetString(0);
                        window.ModelDevice.Text = Model;
                        window.WorkTextBox.Text = WorkName;
                        window.Price.Text = WorkPrice;
                    }
                }

                window.ModelDevice.IsEnabled = false;
                window.ShowDialog();
            }
        }

        private void DevicesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                DataGridRow dataGridRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                DataRowView rowView = (DataRowView)dataGridRow.Item;

                string BrandName = rowView[0].ToString();
                string TypeName = rowView[1].ToString();
                string Model = rowView[2].ToString();
                ShowAndEditDevice window = new ShowAndEditDevice();

                /*string dbcon = @"Data Source = AddOrder.db; Version=3;";
                string SelectModel = "SELECT Model FROM Devices WHERE id = @DeviceID";

                using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                {
                    connection.Open();
                    SQLiteCommand SelectModel_command = new SQLiteCommand(SelectModel, connection);
                    SelectModel_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceID;

                    using (SQLiteDataReader reader = SelectModel_command.ExecuteReader())
                    {
                        reader.Read();
                        string Model = reader.GetString(0);
                        window.ModelDevice.Text = Model;
                        window.WorkTextBox.Text = WorkName;
                        window.Price.Text = WorkPrice;
                    }
                }*/

                window.BrandComboBox.Text = BrandName;
                window.TypeDevice.Text = TypeName;
                window.ModelDevice.Text = Model;
                window.ModelDevice.IsEnabled = false;
                window.ShowDialog();
            }
        }
    }
}

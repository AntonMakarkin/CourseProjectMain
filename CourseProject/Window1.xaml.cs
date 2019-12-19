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

        private void Window_Loaded(object sender, EventArgs e)
        {
            string dbcon = "Data Source = AddOrder.db; Version = 3";
            string query_order = "SELECT Name, Model, WorkName, MasterName, Date, Status FROM Application a JOIN Clients c ON a.ClientId = c.id JOIN Devices d ON a.DeviceId = d.id JOIN Works w ON a.WorkId = w.id JOIN Masters m  ON a.MasterId = m.id";
            string query_client = "SELECT Name,Phone FROM Clients";
            string query_devices = "SELECT Types.Name, Brands.BrandName, Devices.Model From Brands INNER JOIN(Types INNER JOIN Devices ON Types.id = Devices.TypeId) ON Brands.id = Devices.BrandId";
            string query_master = "SELECT MasterName,Phone,IDNumber FROM Masters";
            string query_works = "SELECT Works.WorkName, Works.Price, Works.DeviceID, Devices.id, Devices.Model FROM Works, Devices WHERE DeviceID = Devices.id;";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_command_order = new SQLiteCommand(query_order, connection);
                SQLiteCommand select_command_order_ended = new SQLiteCommand(query_order, connection);
                SQLiteCommand select_command_client = new SQLiteCommand(query_client, connection);
                SQLiteCommand select_command_devices = new SQLiteCommand(query_devices, connection);
                SQLiteCommand select_command_master = new SQLiteCommand(query_master, connection);
                SQLiteCommand select_command_works = new SQLiteCommand(query_works, connection);
                using (SQLiteDataReader reader = select_command_order.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    OrdersDataGrid.ItemsSource = dt.DefaultView;
                    
                }
                using (SQLiteDataReader reader = select_command_client.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    ClientsDataGrid.ItemsSource = dt.DefaultView;
                }
                using (SQLiteDataReader reader = select_command_devices.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    DevicesDataGrid.ItemsSource = dt.DefaultView;
                }
                using (SQLiteDataReader reader = select_command_master.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    MasterDataGrid.ItemsSource = dt.DefaultView;
                }
                using (SQLiteDataReader reader = select_command_client.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    WorksDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void ButtonCloseMain(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void CmdMinimizeMain_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void AddClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Main.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Visible;
            Work.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Collapsed;
        }

        private void Masters_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Visible;
            Work.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Collapsed;
        }

        private void AddNewOrder_Click(object sender, RoutedEventArgs e)
        {
            AddOrder add = new AddOrder();
            add.ShowDialog();
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
        }

        private void Devices_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Visible;
            Work.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Collapsed;
        }

        private void Orders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Main.Visibility = Visibility.Collapsed;
            Master.Visibility = Visibility.Collapsed;
            Devices.Visibility = Visibility.Collapsed;
            Clients.Visibility = Visibility.Collapsed;
            Work.Visibility = Visibility.Collapsed;
            ActualOrders.Visibility = Visibility.Visible;
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
                window.Owner = this;
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
                window.Owner = this;
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

                string TypeName = rowView[0].ToString();
                string BrandName = rowView[1].ToString();
                string Model = rowView[2].ToString();
                ShowAndEditDevice window = new ShowAndEditDevice();

                window.BrandComboBox.Text = BrandName;
                window.TypeDevice.Text = TypeName;
                window.ModelDevice.Text = Model;
                window.BrandComboBox.IsEnabled = false;
                window.TypeDevice.IsEnabled = false;
                window.ModelDevice.IsReadOnly = true;
                window.ShowDialog();
            }
        }

        private void OrdersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
                DataGridRow dataGridRow = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                DataRowView rowView = (DataRowView)dataGridRow.Item;

                string ClientName = rowView[0].ToString();
                string ModelName = rowView[1].ToString();
                string WorkName = rowView[2].ToString();
                string MasterName = rowView[3].ToString();
                string Date = rowView[4].ToString();
                string Status = rowView[5].ToString();
                ShowAndEditOrder window = new ShowAndEditOrder();

                window.ClientComboBox.Text = ClientName;
                window.DeviceModel.Text = ModelName;
                window.TypeOfWork.Text = WorkName;
                window.MasterName.Text = MasterName;
                window.Date.Text = Date;
                window.Status.Text = Status;
                window.ShowDialog();
            }
        }

        public void DataRefreshClients()
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
                    ClientsDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void DevicesRefresh()
        {
            string dbcon = "Data Source = AddOrder.db; Version = 3";
            string query_master = "SELECT MasterName,Phone,IDNumber FROM Masters";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_command = new SQLiteCommand(query_master, connection);
                using (SQLiteDataReader reader = select_command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    DevicesDataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void MasterRefresh()
        {
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
    }
}

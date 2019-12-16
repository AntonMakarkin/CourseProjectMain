using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;
using MessageBox = System.Windows.MessageBox;

namespace CourseProject
{
    /// <summary>
    /// Логика взаимодействия для ShowAndEditWork.xaml
    /// </summary>
    public partial class ShowAndEditWork : Window
    {
        public static string WorkName;
        public static string Model;
        public static string Cost;
        public static int WorkID;
        public static int DeviceIDdb;

        public ShowAndEditWork()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddModels();
        }

        private void AddModels()
        {
            string dbcon = @"Data Source = AddOrder.db; Version=3;";
            string query = "SELECT Model FROM Devices;";
            string search_device_id = "SELECT id FROM Devices WHERE Model = @ModelName";
            string search_workId = "SELECT id FROM Works WHERE WorkName = @WorkName AND DeviceID = @DeviceID";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand add_models_of_devices = new SQLiteCommand(query, connection);

                using (SQLiteDataReader models_reader = add_models_of_devices.ExecuteReader())
                {
                    while (models_reader.Read())
                    {
                        ModelDevice.Items.Add(models_reader["Model"]);
                    }
                }

                SQLiteCommand search_device_id_command = new SQLiteCommand(search_device_id, connection);
                search_device_id_command.Parameters.Add("@ModelName", DbType.String).Value = ModelDevice.Text;

                using (SQLiteDataReader DeviceIdReader = search_device_id_command.ExecuteReader())
                {
                    DeviceIdReader.Read();
                    int DeviceID = DeviceIdReader.GetInt32(0);
                    DeviceIDdb = DeviceID;
                }

                SQLiteCommand search_workId_command = new SQLiteCommand(search_workId, connection);
                search_workId_command.Parameters.Add("@WorkName", DbType.String).Value = WorkTextBox.Text;
                search_workId_command.Parameters.Add("@DeviceID", DbType.String).Value = DeviceIDdb;

                using (SQLiteDataReader WorkIdReader = search_workId_command.ExecuteReader())
                {
                    WorkIdReader.Read();
                    int WorkId = WorkIdReader.GetInt32(0);
                    WorkID = WorkId;
                }
            }

        }

        private void ButtonCloseMain(object sender, EventArgs e)
        {
            Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            WorkName = WorkTextBox.Text;
            Model = ModelDevice.Text;
            Cost = Price.Text;
            WorkTextBox.IsReadOnly = false;
            ModelDevice.IsEnabled = true;
            Price.IsReadOnly = false;
            Edit.Visibility = Visibility.Collapsed;
            Return.Visibility = Visibility.Visible;
            Delete.Visibility = Visibility.Collapsed;
            Save.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteClientModalWindow window = new DeleteClientModalWindow();
            window.MainText.Text = "Вы действительно хотите удалить данную услугу?";

            if (window.ShowDialog() == true)
            {
                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                string delete_work = "DELETE FROM Works WHERE id = @WorkID";

                using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                {
                    connection.Open();
                    SQLiteCommand delete_work_command = new SQLiteCommand(delete_work, connection);
                    delete_work_command.Parameters.Add("@WorkID", DbType.Int32).Value = WorkID;
                    delete_work_command.ExecuteNonQuery();
                }
                
                Hide();

                string message = "Услуга удалена";
                string caption = "Сообщение";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(message, caption, button, icon);

            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (WorkTextBox.Text.Trim() == "" || ModelDevice.Text.Trim() == "" || Price.Text.Trim() == "")
            {
                attention.Visibility = Visibility.Visible;
            }
            else
            {
                attention.Visibility = Visibility.Hidden;
                if(WorkTextBox.Text == WorkName && ModelDevice.Text == Model && Price.Text == Cost)
                {
                    Return.Visibility = Visibility.Collapsed;
                    WorkTextBox.IsReadOnly = true;
                    ModelDevice.IsEnabled = false;
                    Price.IsReadOnly = true;
                    WorkTextBox.Text = WorkName;
                    ModelDevice.Text = Model;
                    Price.Text = Cost;
                    Edit.Visibility = Visibility.Visible;
                    Delete.Visibility = Visibility.Visible;
                    Return.Visibility = Visibility.Collapsed;
                    Save.Visibility = Visibility.Collapsed;
                    attention.Visibility = Visibility.Hidden;
                    attention_1.Visibility = Visibility.Hidden;
                }
                else
                {
                    string PriceNumber = Price.Text.Trim();
                    int PriceNumberInt = int.Parse(PriceNumber);

                    string dbcon = @"Data Source = AddOrder.db; Version=3;";
                    string search_device_id = "SELECT id FROM Devices WHERE Model = @ModelName";
                    string work_search = "SELECT id FROM Works WHERE WorkName = @Name AND Price = @WorkPrice AND DeviceID = @DeviceID AND id != @WorkID";
                    string work_update = "UPDATE Works SET WorkName = @WorkName, Price = @WorkPrice, DeviceID = @DeviceID WHERE id = @WorkID";

                    using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                    {
                        connection.Open();
                        SQLiteCommand search_device_id_command = new SQLiteCommand(search_device_id, connection);
                        search_device_id_command.Parameters.Add("@ModelName", DbType.String).Value = ModelDevice.Text;

                        using (SQLiteDataReader DeviceIdReader = search_device_id_command.ExecuteReader())
                        {
                            DeviceIdReader.Read();
                            int DeviceIDForQuery = DeviceIdReader.GetInt32(0);
                            DeviceIDdb = DeviceIDForQuery;
                        }

                        SQLiteCommand work_search_command = new SQLiteCommand(work_search, connection);
                        work_search_command.Parameters.Add("@Name", DbType.String).Value = WorkTextBox.Text;
                        work_search_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceIDdb;
                        work_search_command.Parameters.Add("@WorkID", DbType.Int32).Value = WorkID;
                        work_search_command.Parameters.Add("@WorkPrice", DbType.Int32).Value = PriceNumberInt;

                        SQLiteDataAdapter da = new SQLiteDataAdapter(work_search_command);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            attention_1.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            attention_1.Visibility = Visibility.Hidden;
                            SQLiteCommand work_update_command = new SQLiteCommand(work_update, connection);
                            work_update_command.Parameters.Add("@WorkName", DbType.String).Value = WorkTextBox.Text;
                            work_update_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceIDdb;
                            work_update_command.Parameters.Add("@WorkID", DbType.Int32).Value = WorkID;
                            work_update_command.Parameters.Add("@WorkPrice", DbType.Int32).Value = PriceNumberInt;
                            work_update_command.ExecuteNonQuery();

                            string message = "Данные успешно изменены";
                            string caption = "Сообщение";
                            MessageBoxButton button = MessageBoxButton.OK;
                            MessageBoxImage icon = MessageBoxImage.Information;
                            MessageBox.Show(message, caption, button, icon);
                            Close();
                        }
                    }
                }
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Return.Visibility = Visibility.Collapsed;
            WorkTextBox.IsReadOnly = true;
            ModelDevice.IsEnabled = false;
            Price.IsReadOnly = true;
            WorkTextBox.Text = WorkName;
            ModelDevice.Text = Model;
            Price.Text = Cost;
            Edit.Visibility = Visibility.Visible;
            Delete.Visibility = Visibility.Visible;
            Return.Visibility = Visibility.Collapsed;
            Save.Visibility = Visibility.Collapsed;
            attention.Visibility = Visibility.Hidden;
            attention_1.Visibility = Visibility.Hidden;
        }

    }
}

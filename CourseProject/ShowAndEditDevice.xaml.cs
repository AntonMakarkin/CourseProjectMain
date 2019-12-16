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
    /// Логика взаимодействия для ShowAndEditDevice.xaml
    /// </summary>
    public partial class ShowAndEditDevice : Window
    {
        public static string Brand;
        public static string Type;
        public static string Model;
        public static int DeviceID;
        public static int BrandID;
        public static int TypeID;

        public ShowAndEditDevice()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string dbcon = @"Data Source = AddOrder.db; Version=3;";
            string search_device_id = "SELECT id FROM Devices WHERE Model = @ModelName";
            string show_brand = "SELECT BrandName FROM Brands";
            string type_show = "SELECT Name FROM Types";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand search_device_id_command = new SQLiteCommand(search_device_id, connection);
                search_device_id_command.Parameters.Add("@ModelName", DbType.String).Value = ModelDevice.Text.Trim();

                using (SQLiteDataReader DeviceIDReader = search_device_id_command.ExecuteReader())
                {
                    DeviceIDReader.Read();
                    int IDofDevice = DeviceIDReader.GetInt32(0);
                    DeviceID = IDofDevice;
                }

                SQLiteCommand show_brand_command = new SQLiteCommand(show_brand, connection);

                using (SQLiteDataReader BrandDeviceReader = show_brand_command.ExecuteReader())
                {
                    while (BrandDeviceReader.Read())
                    {
                        BrandComboBox.Items.Add(BrandDeviceReader["BrandName"]);
                    }
                }

                SQLiteCommand type_show_command = new SQLiteCommand(type_show, connection);

                using (SQLiteDataReader TypeDeviceReader = type_show_command.ExecuteReader())
                {
                    while (TypeDeviceReader.Read())
                    {
                        TypeDevice.Items.Add(TypeDeviceReader["Name"]);
                    }
                }
            }
        }

        private void ButtonCloseMain(object sender, EventArgs e)
        {
            Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Brand = BrandComboBox.Text;
            Type = TypeDevice.Text;
            Model = ModelDevice.Text;
            BrandComboBox.IsEnabled = true;
            TypeDevice.IsEnabled = true;
            ModelDevice.IsReadOnly = false;
            Edit.Visibility = Visibility.Collapsed;
            Return.Visibility = Visibility.Visible;
            Delete.Visibility = Visibility.Collapsed;
            Save.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteClientModalWindow window = new DeleteClientModalWindow();
            window.MainText.Text = "Вы действительно хотите удалить данный девайс?";

            if (window.ShowDialog() == true)
            {
                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                string delete_device = "DELETE FROM Devices WHERE id = @DeviceID";

                using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                {
                    connection.Open();
                    SQLiteCommand delete_device_command = new SQLiteCommand(delete_device, connection);
                    delete_device_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceID;
                    delete_device_command.ExecuteNonQuery();
                }

                Hide();

                string message = "Девайс удален";
                string caption = "Сообщение";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(message, caption, button, icon);
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            BrandComboBox.Text = Brand;
            TypeDevice.Text = Type;
            ModelDevice.Text = Model;
            BrandComboBox.IsEnabled = false;
            TypeDevice.IsEnabled = false;
            ModelDevice.IsReadOnly = true;
            Edit.Visibility = Visibility.Visible;
            Delete.Visibility = Visibility.Visible;
            Return.Visibility = Visibility.Collapsed;
            Save.Visibility = Visibility.Collapsed;
            attention.Visibility = Visibility.Hidden;
            attention_1.Visibility = Visibility.Hidden;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (BrandComboBox.Text == "" || TypeDevice.Text == "" || ModelDevice.Text == "")
            {
                attention.Visibility = Visibility.Visible;
            }
            else
            {
                attention.Visibility = Visibility.Hidden;
                if (BrandComboBox.Text == Brand && TypeDevice.Text == Type && ModelDevice.Text == Model)
                {
                    BrandComboBox.Text = Brand;
                    TypeDevice.Text = Type;
                    ModelDevice.Text = Model;
                    BrandComboBox.IsEnabled = false;
                    TypeDevice.IsEnabled = false;
                    ModelDevice.IsReadOnly = true;
                    Edit.Visibility = Visibility.Visible;
                    Delete.Visibility = Visibility.Visible;
                    Return.Visibility = Visibility.Collapsed;
                    Save.Visibility = Visibility.Collapsed;
                    attention.Visibility = Visibility.Hidden;
                }
                else
                {
                    string dbcon = @"Data Source = AddOrder.db; Version=3;";
                    string brand_id_search = "SELECT id FROM Brands WHERE BrandName = @Name";
                    string type_id_search = "SELECT id FROM Types WHERE Name = @Name";
                    string device_search = "SELECT id FROM Devices WHERE TypeId = @TypeID AND BrandId = @BrandID AND Model = @Model AND id != @DeviceID";
                    string device_update = "UPDATE Devices SET TypeId = @TypeID, BrandId = @BrandID, Model = @Model WHERE id = @DeviceID";

                    using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                    {
                        connection.Open();
                        SQLiteCommand brand_id_search_command = new SQLiteCommand(brand_id_search, connection);
                        brand_id_search_command.Parameters.Add("@Name", DbType.String).Value = BrandComboBox.Text;

                        using (SQLiteDataReader BrandIdReader = brand_id_search_command.ExecuteReader())
                        {
                            BrandIdReader.Read();
                            int BrandId = BrandIdReader.GetInt32(0);
                            BrandID = BrandId;
                        }

                        SQLiteCommand type_id_search_command = new SQLiteCommand(type_id_search, connection);
                        type_id_search_command.Parameters.Add("@Name", DbType.String).Value = TypeDevice.Text;

                        using (SQLiteDataReader TypeIdReader = type_id_search_command.ExecuteReader())
                        {
                            TypeIdReader.Read();
                            int TypeId = TypeIdReader.GetInt32(0);
                            TypeID = TypeId;
                        }

                        SQLiteCommand device_search_command = new SQLiteCommand(device_search, connection);
                        device_search_command.Parameters.Add("@TypeID", DbType.Int32).Value = TypeID;
                        device_search_command.Parameters.Add("@BrandID", DbType.Int32).Value = BrandID;
                        device_search_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceID;
                        device_search_command.Parameters.Add("@Model", DbType.String).Value = ModelDevice.Text;

                        SQLiteDataAdapter da = new SQLiteDataAdapter(device_search_command);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if(dt.Rows.Count > 0)
                        {
                            attention_1.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            attention_1.Visibility = Visibility.Hidden;
                            SQLiteCommand device_update_command = new SQLiteCommand(device_update, connection);
                            device_update_command.Parameters.Add("@TypeID", DbType.Int32).Value = TypeID;
                            device_update_command.Parameters.Add("@BrandID", DbType.Int32).Value = BrandID;
                            device_update_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceID;
                            device_update_command.Parameters.Add("@Model", DbType.String).Value = ModelDevice.Text;
                            device_update_command.ExecuteNonQuery();

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

        private void Save_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

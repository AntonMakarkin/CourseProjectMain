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
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;
using System.Configuration;

namespace CourseProject
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        public AddOrder()
        {
            InitializeComponent();
            Add_Order();
        }

       

        void Add_Order()
        {
            //Добавляем имена клиентов в combobox
            string dbcon = @"Data Source = AddOrder.db; Version=3;";
            string query = "SELECT Name,Phone FROM Clients ORDER BY Name ASC;";
            SQLiteConnection conn = new SQLiteConnection(dbcon);
            conn.Open();
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ClientComboBox.Items.Add(reader["Name"]);
            }
            reader.Close();


            //Добавляем названия брендов в combobox
            string brand_query = "SELECT * FROM Brands;";
            SQLiteConnection connection = new SQLiteConnection(dbcon);
            connection.Open();
            SQLiteCommand select_brand = new SQLiteCommand(brand_query, connection);
            SQLiteDataAdapter brandDA = new SQLiteDataAdapter(select_brand);
            DataTable BrandDT = new DataTable();
            brandDA.Fill(BrandDT);
            Brand.SelectedValuePath = "id";
            Brand.DisplayMemberPath = "BrandName";
            Brand.ItemsSource = BrandDT.DefaultView;
            TypeOfDevice.IsEnabled = false;
            Model.IsEnabled = false;


            //Добавляем мастеров в combobox
            string master_query = "SELECT Name FROM Masters;";
            SQLiteCommand master_command = new SQLiteCommand(master_query, conn);
            SQLiteDataReader master_reader = master_command.ExecuteReader();

            while (master_reader.Read())
            {
                Master.Items.Add(master_reader["Name"]);
            }
            master_reader.Close();
            conn.Close();
        }


        private void ButtonCloseMain(object sender, EventArgs e)
        {
            Close();
        }

        public void Brand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Добавляем названия типов в combobox
            /*TypeOfDevice.IsEnabled = true; //разблокируем combobox типов
            Model.IsEnabled = false; //combobox моделей остается заблокирован
            string dbcon = @"Data Source = AddOrder.db; Version=3;"; //путь к бд
            string type_query = "SELECT Name FROM Types;"; //запрос для извлечения типов устройства
            SQLiteConnection conn = new SQLiteConnection(dbcon); //подсоединяемся к бд
            conn.Open(); //открываем соединение

            SQLiteCommand type_command = new SQLiteCommand(type_query, conn);
            //SQLiteDataReader type_reader = type_command.ExecuteReader();
            SQLiteDataAdapter type_DA = new SQLiteDataAdapter(type_command);
            DataTable typeDT = new DataTable();
            type_DA.Fill(typeDT);
            if (typeDT.Rows.Count > 0)
            {
                
                SQLiteDataReader type_reader = type_command.ExecuteReader();
                TypeOfDevice.Items.Clear();
                //Model.Items.Clear();
                while (type_reader.Read()) //пока считываем таблицу
                {
                    TypeOfDevice.Items.Add(type_reader["Name"]); //добаляем найденные строчки
                }
                type_reader.Close(); //заканчиваем прочтение
            }
            else
            {

            }*/
            //conn.Close();
            if (Brand.SelectedValue.ToString() != null)
            {
                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                SQLiteConnection conn = new SQLiteConnection(dbcon);
                string type_query_1 = "SELECT DISTINCT Devices.TypeId, Devices.BrandId, Types.id, Types.Name FROM Devices, Types WHERE Devices.TypeId = Types.id AND Devices.BrandID = @BrandID";
                //string type_query_1 = "SELECT DISTINCT BrandID, Name FROM Types WHERE BrandID = @ID;";
                SQLiteCommand type_query_command = new SQLiteCommand(type_query_1, conn);
                conn.Open();

                type_query_command.Parameters.AddWithValue("@BrandID", Brand.SelectedValue.ToString());
                SQLiteDataAdapter TypeDA = new SQLiteDataAdapter(type_query_command);
                DataTable TypeDT = new DataTable();
                TypeDA.Fill(TypeDT);
                TypeOfDevice.SelectedValuePath = "id";
                TypeOfDevice.DisplayMemberPath = "Name";
                TypeOfDevice.ItemsSource = TypeDT.DefaultView;
                TypeOfDevice.IsEnabled = true;
                Model.IsEnabled = false;
            }

        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            /*if (TypeOfDevice.SelectedValue.ToString() != null)
            {
                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                SQLiteConnection conn = new SQLiteConnection(dbcon);
                string type_query_1 = "SELECT DISTINCT Devices.TypeId, Devices.BrandId, Types.id, Types.Name FROM Devices, Types WHERE Devices.TypeId = Types.id AND Devices.BrandID = @BrandID";
                //string type_query_1 = "SELECT DISTINCT BrandID, Name FROM Types WHERE BrandID = @ID;";
                SQLiteCommand type_query_command = new SQLiteCommand(type_query_1, conn);
                conn.Open();

                //type_query_command.Parameters.AddWithValue("@ID", Brand.SelectedValue.ToString());
                type_query_command.Parameters.AddWithValue("@BrandID", Brand.SelectedValue.ToString());
                SQLiteDataAdapter TypeDA = new SQLiteDataAdapter(type_query_command);
                DataTable TypeDT = new DataTable();
                TypeDA.Fill(TypeDT);
                Model.SelectedValuePath = "id";
                Model.DisplayMemberPath = "Name";
                TypeOfDevice.ItemsSource = TypeDT.DefaultView;
                TypeOfDevice.IsEnabled = true;
                Model.IsEnabled = false;
            }*/
            /*if (TypeOfDevice.SelectedValue.ToString() != null)
            {
                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                SQLiteConnection conn = new SQLiteConnection(dbcon);
                string model_search = "SELECT * FROM Devices WHERE TypeId = @TypeID AND BrandId = @BrandID";
                //string model_search = "SELECT * FROM Models WHERE TypeID = @id;";
                SQLiteCommand model_search_command = new SQLiteCommand(model_search, conn);
                conn.Open();

                model_search_command.Parameters.AddWithValue("@TypeID", TypeOfDevice.SelectedValue.ToString());
                model_search_command.Parameters.AddWithValue("@BrandID", Brand.SelectedValue.ToString());
                SQLiteDataAdapter ModelDA = new SQLiteDataAdapter(model_search_command);
                DataTable ModelDT = new DataTable();
                ModelDA.Fill(ModelDT);
                Model.SelectedValuePath = "id";
                Model.DisplayMemberPath = "Model";
                Model.ItemsSource = ModelDT.DefaultView;
                //Brand.IsEnabled = false;
                Model.IsEnabled = true;
                Work.IsEnabled = false;
            }*/
        }

        private void Model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            /*TypeOfDevice.IsEnabled = false;
            Model.IsEnabled = false;
            TypeOfDevice.Text = "";
            Model.Text = "";*/
            Hide();
            AddOrder add = new AddOrder();
            add.ShowDialog();
        }
    }
}

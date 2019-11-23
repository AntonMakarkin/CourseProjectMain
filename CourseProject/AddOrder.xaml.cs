using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

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
            /*string dbcon = @"Data Source = AddOrder.db; Version=3;";
            string query = "SELECT Name,Phone FROM Clients ORDER BY Name ASC;";
            SQLiteConnection conn = new SQLiteConnection(dbcon);
            conn.Open();
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ClientComboBox.Items.Add(reader["Name"]);
            }
            reader.Close();*/

            string dbcon = @"Data Source = AddOrder.db; Version=3;";
            string query = "SELECT Name,Phone FROM Clients ORDER BY Name ASC;";
            SQLiteConnection conn = new SQLiteConnection(dbcon);
            conn.Open();
            SQLiteCommand select_client = new SQLiteCommand(query, conn);
            SQLiteDataAdapter ClientDA = new SQLiteDataAdapter(select_client);
            DataTable ClientDT = new DataTable();
            ClientDA.Fill(ClientDT);
            ClientComboBox.SelectedValuePath = "id";
            ClientComboBox.DisplayMemberPath = "Name";
            ClientComboBox.ItemsSource = ClientDT.DefaultView;

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

            if (TypeOfDevice.SelectedValue.ToString() != null)
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
                Brand.IsEnabled = false;

            }
        }

        private void Model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string dbcon = @"Data Source = AddOrder.db; Version=3;";
            SQLiteConnection conn = new SQLiteConnection(dbcon);
            string work_search = "SELECT * FROM Works WHERE DeviceID = @DeviceID";
            SQLiteCommand work_search_command = new SQLiteCommand(work_search, conn);
            conn.Open();

            work_search_command.Parameters.AddWithValue("@DeviceID", Model.SelectedValue.ToString());
            SQLiteDataAdapter WorkDA = new SQLiteDataAdapter(work_search_command);
            DataTable WorkDT = new DataTable();
            WorkDA.Fill(WorkDT);
            Work.SelectedValuePath = "id";
            Work.DisplayMemberPath = "Name";
            Work.ItemsSource = WorkDT.DefaultView;
            Work.IsEnabled = true;
            TypeOfDevice.IsEnabled = false;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Brand.IsEnabled = false;
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ClientComboBox.SelectedIndex > -1 && Brand.SelectedIndex > -1 && TypeOfDevice.SelectedIndex > -1 && Model.SelectedIndex > -1 && Master.SelectedIndex > -1 && Work.SelectedIndex > -1)
            {
                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                string brand_id_search = "SELECT id FROM Brands WHERE BrandName = @Name";
                string type_id_search = "SELECT id FROM Types WHERE Name = @Name";
                string device_search_for_id = "SELECT id FROM Devices WHERE TypeId = @TypeID AND BrandId = @BrandID AND Model = @Model";
                string work_search_for_id = "SELECT id FROM Works WHERE DeviceID = @DeviceID";
                string client_search_for_id = "SELECT id FROM Clients WHERE Name = @Name";
                string master_search_for_id = "SELECT id FROM Masters WHERE Name = @Name";
                string order_search = "SELECT * FROM Application WHERE ClientId = @ClientID AND DeviceId = @DeviceID AND WorkId = @WorkID AND MasterId = @MasterID";
                //string order_add = "INSERT INTO Application (ClientId, DeviceId, WorkId, MasterId) VALUES (@ClientID, @DeviceID, @WorkID, @MasterID)";
                string order_add = "INSERT INTO Application (ClientId, DeviceId, WorkId, MasterId, Date, Status) VALUES (@ClientID, @DeviceID, @WorkID, @MasterID, @Date, @Status)";

                SQLiteConnection conn = new SQLiteConnection(dbcon);
                conn.Open();

                /*SQLiteCommand device_search_for_id_command = new SQLiteCommand(device_search_for_id, conn);
                device_search_for_id_command.Parameters.AddWithValue("@TypeID", TypeOfDevice.SelectedValue.ToString());
                device_search_for_id_command.Parameters.AddWithValue("@BrandID", Brand.SelectedValue.ToString());
                device_search_for_id_command.Parameters.AddWithValue("@Model", Model.SelectedValue.ToString());
                SQLiteDataReader reader_id_device = device_search_for_id_command.ExecuteReader();
                reader_id_device.Read();
                int DeviceID = reader_id_device.GetInt32(0);
                reader_id_device.Close();*/

                //ищем id бренда
                SQLiteCommand brand_id_search_command = new SQLiteCommand(brand_id_search, conn);
                brand_id_search_command.Parameters.Add("@Name", DbType.String).Value = Brand.Text;
                SQLiteDataReader reader_id_brand = brand_id_search_command.ExecuteReader();
                reader_id_brand.Read();
                int BrandID = reader_id_brand.GetInt32(0);
                reader_id_brand.Close();

                //ищем id типа
                SQLiteCommand type_id_search_command = new SQLiteCommand(type_id_search, conn);
                type_id_search_command.Parameters.Add("@Name", DbType.String).Value = TypeOfDevice.Text;
                SQLiteDataReader reader_id_type = type_id_search_command.ExecuteReader();
                reader_id_type.Read();
                int TypeID = reader_id_type.GetInt32(0);
                reader_id_brand.Close();

                //ищем id девайса
                SQLiteCommand device_search_for_id_command = new SQLiteCommand(device_search_for_id, conn);
                device_search_for_id_command.Parameters.Add("@TypeID", DbType.Int32).Value = TypeID;
                device_search_for_id_command.Parameters.Add("@BrandID", DbType.Int32).Value = BrandID;
                device_search_for_id_command.Parameters.Add("@Model", DbType.String).Value = Model.Text;
                SQLiteDataReader reader_id_device = device_search_for_id_command.ExecuteReader();
                reader_id_device.Read();
                int DeviceID = reader_id_device.GetInt32(0);
                reader_id_device.Close();

                //ищем id услуги
                SQLiteCommand work_search_for_id_command = new SQLiteCommand(work_search_for_id, conn);
                work_search_for_id_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceID;
                SQLiteDataReader reader_work_id = work_search_for_id_command.ExecuteReader();
                reader_work_id.Read();
                int WorkID = reader_work_id.GetInt32(0);
                reader_work_id.Close();

                //ищем id клиента
                SQLiteCommand client_search_for_id_command = new SQLiteCommand(client_search_for_id, conn);
                client_search_for_id_command.Parameters.Add("@Name", DbType.String).Value = ClientComboBox.Text;
                SQLiteDataReader reader_client_id = client_search_for_id_command.ExecuteReader();
                reader_client_id.Read();
                int ClientID = reader_client_id.GetInt32(0);
                reader_client_id.Close();

                //ищем id мастера
                SQLiteCommand master_search_for_id_command = new SQLiteCommand(master_search_for_id, conn);
                master_search_for_id_command.Parameters.Add("@Name", DbType.String).Value = Master.Text;
                SQLiteDataReader reader_master_id = master_search_for_id_command.ExecuteReader();
                reader_master_id.Read();
                int MasterID = reader_master_id.GetInt32(0);
                reader_master_id.Close();

                //создаем переменную со статусом заявки (по умолчанию - в процессе)
                string Status = "В процессе";

                //создаем переменную с датой
                DateTime currentDate = DateTime.Today;
                CultureInfo RU = new CultureInfo("ru-RU");
                string Date = currentDate.ToString("D", RU);

                //ищем заявку по введенным параметрам
                SQLiteCommand order_search_command = new SQLiteCommand(order_search, conn);
                order_search_command.Parameters.Add("@ClientID", DbType.Int32).Value = ClientID;
                order_search_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceID;
                order_search_command.Parameters.Add("@WorkID", DbType.Int32).Value = WorkID;
                order_search_command.Parameters.Add("@MasterID", DbType.Int32).Value = MasterID;

                SQLiteDataAdapter orderDA = new SQLiteDataAdapter(order_search_command);
                DataTable orderDT = new DataTable();
                orderDA.Fill(orderDT);
                if(orderDT.Rows.Count > 0) //если заявка с такими данными существует
                {
                    System.Windows.Forms.MessageBox.Show("Данная заявка уже существует");
                }
                else //в противном случае - добавляем
                {
                    SQLiteCommand add_order_command = new SQLiteCommand(order_add, conn);
                    add_order_command.Parameters.Add("@ClientID", DbType.Int32).Value = ClientID;
                    add_order_command.Parameters.Add("@DeviceID", DbType.Int32).Value = DeviceID;
                    add_order_command.Parameters.Add("@WorkID", DbType.Int32).Value = WorkID;
                    add_order_command.Parameters.Add("@MasterID", DbType.Int32).Value = MasterID;
                    add_order_command.Parameters.Add("@Date", DbType.String).Value = Date;
                    add_order_command.Parameters.Add("@Status", DbType.String).Value = Status;
                    add_order_command.ExecuteNonQuery();
                    conn.Close();
                    System.Windows.Forms.MessageBox.Show("Заявка добавлена");
                }
            }
        }
    }
}

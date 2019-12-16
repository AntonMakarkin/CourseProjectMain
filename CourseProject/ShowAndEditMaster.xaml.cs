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
    /// Логика взаимодействия для ShowAndEditMaster.xaml
    /// </summary>
    public partial class ShowAndEditMaster : Window
    {
        Window1 ApplicationWindow = new Window1();
        public static string MasterFullName;
        public static string MasterPhoneNumber;
        public static int MasterID;
        public ShowAndEditMaster()
        {
            InitializeComponent();
        }

        private void ButtonCloseMain(object sender, EventArgs e)
        {
            Close();
        }

        //public event EventHandler ButtonClicked;
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            /*if (ButtonClicked != null)
            {
                ButtonClicked(this, EventArgs.Empty);
            }*/

            string NameString = FullName.Text.Trim();
            int CountWords = 0;
            int id = 0;
            bool findSpace = false;//Ставим флаг поиска пробела в false - это позволит 
                                   //избежать ошибок, если в начале строки будут пробелы

            while (id < NameString.Length)
            {
                if (NameString[id] == ' ')
                {
                    //Проверяем - искали-ли мы его.
                    //Если искали, то ставим флаг того, что нашли, 
                    //если не искали (ищем уже не пробел) - просто идём дальше.
                    if (findSpace)
                        findSpace = false;
                }
                //Если нашли не пробел
                else
                {
                    //Если искали пробел, то ничего не делаем, 
                    //а просто идём дальше, в противном случае
                    //Говорим, что мы нашлю символ который не пробел,
                    //и увеличиваем счётчик
                    if (!findSpace)
                    {
                        //Говорим, что теперь ищем снова пробел
                        findSpace = true;
                        //Увеличиваем счётчик слов
                        CountWords++;
                    }
                }
                id++;
            }

            /*string NumberString = PhoneNumber.Text;
            int id_2 = 0;
            bool CorrectNumber = false;
            int NumberLength = NumberString.Length;

            if (NumberLength != 11)
            {
                CorrectNumber = false;
            }
            else
            {
                while (id_2 < NumberLength)
                {
                    if (NumberString[id_2] == ' ')
                    {
                        CorrectNumber = false;
                    }
                    else
                    {
                        CorrectNumber = true;
                    }
                }
            }*/



            if (FullName.Text == "" || PhoneNumber.Text == "")
            {
                Attention_2.Visibility = Visibility.Visible;
            }
            else
            {
                if (CountWords < 3)
                {
                    Attention.Visibility = Visibility.Visible;
                }
                else
                {
                    Attention.Visibility = Visibility.Hidden;
                    if (FullName.Text == MasterFullName && PhoneNumber.Text == MasterPhoneNumber)
                    {
                        Return.Visibility = Visibility.Collapsed;
                        FullName.IsReadOnly = true;
                        PhoneNumber.IsReadOnly = true;
                        FullName.Text = MasterFullName;
                        PhoneNumber.Text = MasterPhoneNumber;
                        Edit.Visibility = Visibility.Visible;
                        Delete.Visibility = Visibility.Visible;
                        Return.Visibility = Visibility.Collapsed;
                        Save.Visibility = Visibility.Collapsed;
                        Attention.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        string dbcon = @"Data Source = AddOrder.db; Version=3;";
                        string master_search = "SELECT id FROM Masters WHERE MasterName = @MasterName AND Phone = @MasterPhone AND id != @MasterID";
                        string master_update = "UPDATE Masters SET MasterName = @MasterName, Phone = @MasterPhone WHERE id = @MasterID";

                        using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                        {
                            connection.Open();

                            SQLiteCommand master_search_command = new SQLiteCommand(master_search, connection);
                            master_search_command.Parameters.Add("@MasterName", DbType.String).Value = FullName.Text;
                            master_search_command.Parameters.Add("@MasterPhone", DbType.String).Value = PhoneNumber.Text;
                            master_search_command.Parameters.Add("@MasterID", DbType.String).Value = MasterID;

                            SQLiteDataAdapter da = new SQLiteDataAdapter(master_search_command);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                Attention_1.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                Attention_1.Visibility = Visibility.Hidden;
                                SQLiteCommand master_update_command = new SQLiteCommand(master_update, connection);
                                master_update_command.Parameters.Add("@MasterName", DbType.String).Value = FullName.Text;
                                master_update_command.Parameters.Add("@MasterPhone", DbType.String).Value = PhoneNumber.Text;
                                master_update_command.Parameters.Add("@MasterID", DbType.Int32).Value = MasterID;
                                master_update_command.ExecuteNonQuery();

                                string message = "Данные успешно изменены";
                                string caption = "Сообщение";
                                MessageBoxButton button = MessageBoxButton.OK;
                                MessageBoxImage icon = MessageBoxImage.Information;
                                MessageBox.Show(message, caption, button, icon);
                                Close();
                            }

                        }

                        /*string query = "SELECT Name,Phone,IDNumber FROM Masters";

                        using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                        {
                            connection.Open();
                            SQLiteCommand select_command = new SQLiteCommand(query, connection);
                            using (SQLiteDataReader reader = select_command.ExecuteReader())
                            {
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                ApplicationWindow.MasterDataGrid.ItemsSource = dt.DefaultView;
                                ApplicationWindow.Show();
                            }
                        }*/
                    }
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            string dbcon = @"Data Source = AddOrder.db; Version=3;";
            string select_id_master = "SELECT id FROM Masters WHERE MasterName = @Name AND Phone = @Phone";

            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_id_master_command = new SQLiteCommand(select_id_master, connection);
                select_id_master_command.Parameters.Add("@Name", DbType.String).Value = FullName.Text;
                select_id_master_command.Parameters.Add("@Phone", DbType.String).Value = PhoneNumber.Text;
                using (SQLiteDataReader MasterIdReader = select_id_master_command.ExecuteReader())
                {
                    MasterIdReader.Read();
                    int MasterId = MasterIdReader.GetInt32(0);
                    MasterID = MasterId;
                }
            }

            MasterFullName = FullName.Text;
            MasterPhoneNumber = PhoneNumber.Text;
            FullName.IsReadOnly = false;
            PhoneNumber.IsReadOnly = false;
            Edit.Visibility = Visibility.Collapsed;
            Return.Visibility = Visibility.Visible;
            Delete.Visibility = Visibility.Collapsed;
            Save.Visibility = Visibility.Visible;
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Return.Visibility = Visibility.Collapsed;
            FullName.IsReadOnly = true;
            PhoneNumber.IsReadOnly = true;
            FullName.Text = MasterFullName;
            PhoneNumber.Text = MasterPhoneNumber;
            Edit.Visibility = Visibility.Visible;
            Delete.Visibility = Visibility.Visible;
            Return.Visibility = Visibility.Collapsed;
            Save.Visibility = Visibility.Collapsed;
            Attention.Visibility = Visibility.Hidden;
            Attention_1.Visibility = Visibility.Hidden;
            Attention_2.Visibility = Visibility.Hidden;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteClientModalWindow window = new DeleteClientModalWindow();
            window.MainText.Text = "Вы действительно хотите удалить данного мастера?";

            if (window.ShowDialog() == true)
            {
                Hide();
                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                string delete_client = "DELETE FROM Masters WHERE MasterName = @Name AND Phone = @Phone";
                
                using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                {
                    connection.Open();
                    SQLiteCommand delete_client_command = new SQLiteCommand(delete_client, connection);
                    delete_client_command.Parameters.Add("@Name", DbType.String).Value = FullName.Text;
                    delete_client_command.Parameters.Add("@Phone", DbType.String).Value = PhoneNumber.Text;
                    delete_client_command.ExecuteNonQuery();
                }

                string message = "Мастер удален";
                string caption = "Сообщение";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(message, caption, button, icon);

            }
        }     
    }
} 

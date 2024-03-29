﻿using System;
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
    /// Логика взаимодействия для ShowAndEditClient.xaml
    /// </summary>
    public partial class Window_Loaded : Window
    {
        public static string ClientFullName;
        public static string ClientPhoneNumber;
        public static int ClientID;
        public Window_Loaded()
        {
            InitializeComponent();
        }

        private void ButtonCloseMain(object sender, EventArgs e)
        {
            Close();
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {

            string NameString = FullName.Text.Trim();
            int CountWords = 0;
            int id = 0;
            bool findSpace = false;

            while (id < NameString.Length)
            {
                if (NameString[id] == ' ')
                {
                    if (findSpace)
                        findSpace = false;
                }
                else
                {
                    if (!findSpace)
                    {
                        findSpace = true;
                        CountWords++;
                    }
                }
                id++;
            }

            if(FullName.Text == "" || PhoneNumber.Text == "")
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
                    if (FullName.Text == ClientFullName && PhoneNumber.Text == ClientPhoneNumber)
                    {
                        Return.Visibility = Visibility.Collapsed;
                        FullName.IsReadOnly = true;
                        PhoneNumber.IsReadOnly = true;
                        FullName.Text = ClientFullName;
                        PhoneNumber.Text = ClientPhoneNumber;
                        Edit.Visibility = Visibility.Visible;
                        Delete.Visibility = Visibility.Visible;
                        Return.Visibility = Visibility.Collapsed;
                        Save.Visibility = Visibility.Collapsed;
                        Attention.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        string dbcon = @"Data Source = AddOrder.db; Version=3;";
                        string client_search = "SELECT id FROM Clients WHERE Name = @ClientName AND Phone = @ClientPhone AND id != @ClientID";
                        string client_update = "UPDATE Clients SET Name = @ClientName, Phone = @ClientPhone WHERE id = @ClientID";

                        using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                        {
                            connection.Open();

                            SQLiteCommand client_search_command = new SQLiteCommand(client_search, connection);
                            client_search_command.Parameters.Add("@ClientName", DbType.String).Value = FullName.Text;
                            client_search_command.Parameters.Add("@ClientPhone", DbType.String).Value = PhoneNumber.Text;
                            client_search_command.Parameters.Add("@ClientID", DbType.String).Value = ClientID;

                            SQLiteDataAdapter da = new SQLiteDataAdapter(client_search_command);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if(dt.Rows.Count > 0)
                            {
                                Attention_1.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                SQLiteCommand client_update_command = new SQLiteCommand(client_update, connection);
                                client_update_command.Parameters.Add("@ClientName", DbType.String).Value = FullName.Text;
                                client_update_command.Parameters.Add("@ClientPhone", DbType.String).Value = PhoneNumber.Text;
                                client_update_command.Parameters.Add("@ClientID", DbType.Int32).Value = ClientID;
                                client_update_command.ExecuteNonQuery();
                                string message = "Данные успешно изменены";
                                string caption = "Сообщение";
                                MessageBoxButton button = MessageBoxButton.OK;
                                MessageBoxImage icon = MessageBoxImage.Information;
                                MessageBox.Show(message, caption, button, icon);
                                Window1 window = this.Owner as Window1;
                                window.DataRefreshClients();
                                Close();
                            }
                        }
                    }
                }
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            string dbcon = @"Data Source = AddOrder.db; Version=3;";
            string select_id_client = "SELECT id FROM Clients WHERE Name = @Name AND Phone = @Phone";
            
            using (SQLiteConnection connection = new SQLiteConnection(dbcon))
            {
                connection.Open();
                SQLiteCommand select_id_client_command = new SQLiteCommand(select_id_client, connection);
                select_id_client_command.Parameters.Add("@Name", DbType.String).Value = FullName.Text;
                select_id_client_command.Parameters.Add("@Phone", DbType.String).Value = PhoneNumber.Text;
                using (SQLiteDataReader ClientIdReader = select_id_client_command.ExecuteReader())
                {
                    ClientIdReader.Read();
                    int ClientId = ClientIdReader.GetInt32(0);
                    ClientID = ClientId;
                }
            }

            ClientFullName = FullName.Text;
            ClientPhoneNumber = PhoneNumber.Text;
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
            FullName.Text = ClientFullName;
            PhoneNumber.Text = ClientPhoneNumber;
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
            

            if(window.ShowDialog() == true)
            {
                Hide();
                string dbcon = @"Data Source = AddOrder.db; Version=3;";
                string delete_client = "DELETE FROM Clients WHERE Name = @Name AND Phone = @Phone";
                
                using (SQLiteConnection connection = new SQLiteConnection(dbcon))
                {
                    connection.Open();
                    SQLiteCommand delete_client_command = new SQLiteCommand(delete_client, connection);
                    delete_client_command.Parameters.Add("@Name", DbType.String).Value = FullName.Text;
                    delete_client_command.Parameters.Add("@Phone", DbType.String).Value = PhoneNumber.Text;
                    delete_client_command.ExecuteNonQuery();
                }

                string message = "Клиент удален";
                string caption = "Сообщение";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(message, caption, button, icon);

                Window1 window1 = this.Owner as Window1;
                window1.DataRefreshClients();

                Close();
            }
        }

    }
}

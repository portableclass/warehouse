using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using warehouse.Entities;

namespace warehouse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBase database;

        public MainWindow()
        {
            InitializeComponent();
            database = new DataBase();
        }

        private void ActionHandler(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "NewBooking")
            {
                Open(NewBookingScreen);
            }
            if (btn.Name == "NewCustomer")
            {
                Open(NewCustomerScreen);
            }
            if (btn.Name == "ChangeCustomerData")
            {
                EditCustomer(null, null);
                Open(ChangeCustomerDataScreen);
            }
            if (btn.Name == "ChangeBookingData")
            {
                EditBooking(null, null);
                Open(ChangeBookingDataScreen);
            }
        }

        private void Open(Border screen)
        {
            MainScreen.Visibility = Visibility.Hidden;
            NewBookingScreen.Visibility = Visibility.Hidden;
            NewCustomerScreen.Visibility = Visibility.Hidden;
            ChangeCustomerDataScreen.Visibility = Visibility.Hidden;
            ChangeBookingDataScreen.Visibility = Visibility.Hidden;

            screen.Visibility = Visibility.Visible;
        }

        private void CreateNewBooking(object sender, RoutedEventArgs e)
        {
            if (NewBookingGoodsId.Text == null || NewBookingCustomerId.Text == null || NewBookingGoodsAmount.Text == null || NewBookingBookingDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "INPUT ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ///////////////////////////////////////
                database.OpenConnection();
                string sel = $"INSERT INTO [dbo].[Booking] ([BookingDate]) VALUES ('{NewBookingBookingDate.Text}')";
                SqlCommand ins = new SqlCommand();
                ins.Connection = database.GetConnection();
                ins.CommandText = sel;
                ins.ExecuteNonQuery();

                string sel2 = "SELECT TOP (1) BookingId From [dbo].[Booking] Order By BookingId Desc";
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = database.GetConnection();
                cmd2.CommandText = sel2;
                var id = cmd2.ExecuteScalar();
                //////////////////////////////////////////////////
                string sqlExpression = "NewBooking";
                SqlCommand cmd = new SqlCommand(sqlExpression, database.GetConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter goodsId = new SqlParameter
                {
                    ParameterName = "@GoodsId",
                    Value = Convert.ToInt32(NewBookingGoodsId.Text)
                };
                SqlParameter bookingId = new SqlParameter
                {
                    ParameterName = "@BookingId",
                    Value = id
                };
                SqlParameter customerId = new SqlParameter
                {
                    ParameterName = "@CustomerId",
                    Value = Convert.ToInt32(NewBookingCustomerId.Text)
                };
                SqlParameter goodsBookingAmount = new SqlParameter
                {
                    ParameterName = "@GoodsBookingAmount",
                    Value = Convert.ToInt32(NewBookingGoodsAmount.Text)
                };
                cmd.Parameters.Add(goodsId);
                cmd.Parameters.Add(bookingId);
                cmd.Parameters.Add(customerId);
                cmd.Parameters.Add(goodsBookingAmount);
                var result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show($"Заказ не был добавлен");
                    string del = $"DELETE FROM [dbo].[Booking] WHERE BookingId = {id}";
                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.Connection = database.GetConnection();
                    cmd3.CommandText = del;
                    cmd3.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show($"Заказ был добавлен. Id созданного заказа: {id}");
                }

            }
        }

        private void CreateNewCustomer(object sender, RoutedEventArgs e)
        {
            database.OpenConnection();
            string sqlExpression = "NewClient";
            SqlCommand cmd = new SqlCommand(sqlExpression, database.GetConnection());
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter customerName = new SqlParameter
            {
                ParameterName = "@CustomerName",
                Value = NewCustomerCustomerName.Text
            };
            SqlParameter customerSurname = new SqlParameter
            {
                ParameterName = "@CustomerSurname",
                Value = NewCustomerCustomerSurname.Text
            };
            SqlParameter customerPatronymic = new SqlParameter
            {
                ParameterName = "@CustomerPatronymic",
                Value = NewCustomerCustomerPatronymic.Text
            };
            SqlParameter customerPhoneNumber = new SqlParameter
            {
                ParameterName = "@CustomerPhoneNumber",
                Value = NewCustomerCustomerPhone.Text
            };
            SqlParameter customerCity = new SqlParameter
            {
                ParameterName = "@CustomerCity",
                Value = NewCustomerCustomerCity.Text
            };
            SqlParameter customerStreet = new SqlParameter
            {
                ParameterName = "@CustomerStreet",
                Value = NewCustomerCustomerStreet.Text
            };
            SqlParameter customerHouse = new SqlParameter
            {
                ParameterName = "@CustomerHouse",
                Value = NewCustomerCustomerHouse.Text
            };
            SqlParameter customerPostalCode = new SqlParameter
            {
                ParameterName = "@CustomerPostalCode",
                Value = Convert.ToInt32(NewCustomerCustomerPostIndex.Text)
            };
            cmd.Parameters.Add(customerName);
            cmd.Parameters.Add(customerSurname);
            cmd.Parameters.Add(customerPatronymic);
            cmd.Parameters.Add(customerPhoneNumber);
            cmd.Parameters.Add(customerCity);
            cmd.Parameters.Add(customerStreet);
            cmd.Parameters.Add(customerHouse);
            cmd.Parameters.Add(customerPostalCode);
            var result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                string sel = "SELECT TOP (1) CustomerId From Customer Order By CustomerId Desc";
                SqlCommand lastId = new SqlCommand();
                lastId.Connection = database.GetConnection();
                lastId.CommandText = sel;
                var id = lastId.ExecuteScalar();
                MessageBox.Show($"Новый клиент был успешно добавлен.\n ID созданного клиента: {id}");
            }
            else
            {
                MessageBox.Show($"Клиент не был добавлен");
            }
        }

        private void EditCustomer(object sender, RoutedEventArgs e)
        {
            ChangeCustomerTable.ItemsSource = WarehouseEntities.GetCtx().Customer.ToList();
        }

        private void EditBooking(object sender, RoutedEventArgs e)
        {
            ChangeBookingTable.ItemsSource = WarehouseEntities.GetCtx().GoodsBooking.ToList();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Open(MainScreen);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            WarehouseEntities.GetCtx().SaveChanges();
            EditBooking(null, null);
            EditCustomer(null, null);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn.Name == "ChangeCustomerDelete")
            {
                if (ChangeCustomerTable.SelectedItems != null)
                {
                    for (int i = 0; i < ChangeCustomerTable.SelectedItems.Count; i++)
                    {
                        string temp = (ChangeCustomerTable.SelectedItems[i] as Customer).CustomerSurname;

                        Customer customer = WarehouseEntities.GetCtx().Customer
                                    .Where(o => o.CustomerSurname == temp)
                                    .FirstOrDefault();

                        WarehouseEntities.GetCtx().Customer.Remove(customer);
                        WarehouseEntities.GetCtx().SaveChanges();

                        EditCustomer(null, null);
                    }
                }
            }
            if (btn.Name == "ChangeBookingDelete")
            {
                if (ChangeBookingTable.SelectedItems != null)
                {
                    for (int i = 0; i < ChangeBookingTable.SelectedItems.Count; i++)
                    {
                        int temp = (int)(ChangeBookingTable.SelectedItems[i] as GoodsBooking).BookingId;

                        GoodsBooking goodsBooking = WarehouseEntities.GetCtx().GoodsBooking
                                    .Where(o => o.BookingId == temp)
                                    .FirstOrDefault();

                        WarehouseEntities.GetCtx().GoodsBooking.Remove(goodsBooking);
                        WarehouseEntities.GetCtx().SaveChanges();

                        EditBooking(null, null);
                    }
                }
            }
        }

    }
}

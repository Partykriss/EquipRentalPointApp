using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Windows;

namespace EquipRentalPointApp.Models
{
    public class DataWorker
    {
        #region GETALLMETHODS
        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetAllCategories", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        categories.Add(new Category()
                        {
                            Id = (int)reader["ID"],
                            Title = (string)reader["Title"]
                        });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            return categories;
        }

        private List<Category> GetCategoriesByEquipId(int equipID)
        {
            List<Category> categories = new List<Category>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetCategoriesByEquipID", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EquipID", equipID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            Id = (int)reader["ID"],
                            Title = (string)reader["Title"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            return categories;
        }

        public List<Equipment> GetAllEquipments()
        {
            List<Equipment> equipments = new List<Equipment>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetAllEquipments", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        List<Category> categories = GetCategoriesByEquipId((int)reader["ID"]);
                        equipments.Add(new Equipment()
                        {
                            Id = (int)reader["ID"],
                            Title = (string)reader["Title"],
                            Price = (decimal)reader["Price"],
                            Categories = categories,
                            CategoryString = GetCategoriesTitleStringByCategoriesList(categories)
                        });
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            return equipments;
        }

        public List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetAllClients", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        clients.Add(new Client()
                        {
                            Id = (int)reader["ID"],
                            FullName = (string)reader["FullName"],
                            Phone = (string)reader["Phone"]
                        });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            return clients;
        }

        public List<Rental> GetAllRentalsNotPayed()
        {
            List<Rental> rentals = new List<Rental>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetAllRentalsNotPayed", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        List<Equipment> equipments = GetEquipmentsByRentalID((int)reader["ID"]);
                        rentals.Add(new Rental()
                        {
                            Id = (int)reader["ID"],
                            Client = GetClientByClientID((int)reader["ClientID"]),
                            Equipments = equipments,
                            EquipmentsString = GetEquipmentsTitleStringByEquiomentsList(equipments),
                            DateBegin = (DateTime)reader["DateBegin"],
                            DateEnd = (DateTime)reader["DateEnd"],
                            Total = (decimal)reader["Total"],
                            Payed = (decimal)reader["Payed"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            return rentals;
        }
        #endregion

        #region GETSTRINGMETHODS
        private string GetCategoriesTitleStringByCategoriesList(List<Category> categories)
        {
            var str = new StringBuilder();
            for (int i = 0; i < categories.Count; i++)
            {
                str.Append(categories[i].Title);
                if (i < categories.Count - 1)
                    str.Append(", ");
            }
            return str.ToString();
        }

        private string GetEquipmentsTitleStringByEquiomentsList(List<Equipment> equipments)
        {
            var str = new StringBuilder();
            for (int i = 0; i < equipments.Count; i++)
            {
                str.Append(equipments[i].Title);
                if (i < equipments.Count - 1)
                    str.Append(", ");
            }
            return str.ToString();
        }
        #endregion

        #region GETBYIDMETHODS
        public List<Equipment> GetEquipmentsByRentalID(int rentalId)
        {
            List<Equipment> equipments = new List<Equipment>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spGetEquipmentsByRentalID", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RentalID", rentalId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    equipments.Add(new Equipment
                    {
                        Id = (int)reader["ID"],
                        Title = (string)reader["Title"],
                        Price = (decimal)reader["Price"]
                    });
                reader.Close();
                connection.Close();
            }
            return equipments;
        }

        public Client GetClientByClientID(int clientId)
        {
            Client client = null;
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spGetClientByClientID", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientID", clientId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    client = new Client()
                    {
                        Id = (int)reader["ID"],
                        FullName = (string)reader["FullName"],
                        Phone = (string)reader["Phone"]
                    };
                reader.Close();
                connection.Close();
            }
            return client;
        }
        #endregion

        #region ADDMETHODS
        public void AddCategory(Category category)
        {
            if (category.Title != string.Empty)
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                {
                    connection.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand("spAddCategory", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Title", category.Title);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Категория {category.Title} добавлена.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    connection.Close();
                }
            }
            else
                MessageBox.Show("Введите название категории");
        }

        public void AddEquipment(Equipment equipment)
        {
            if (equipment.Title != string.Empty)
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                {
                    connection.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand("spAddEquipment", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Title", equipment.Title);
                        cmd.Parameters.AddWithValue("@Price", equipment.Price);
                        for (int i = 1; i <= equipment.Categories.Count; i++)
                            cmd.Parameters.AddWithValue($"@Category{i}ID", equipment.Categories[i - 1].Id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Инвентарь {equipment.Title} добавлен.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    connection.Close();
                }
            }
            else
                MessageBox.Show("Введите название инвентаря");
        }

        public void AddClient(Client client)
        {
            if (client.FullName != string.Empty)
            {
                using (var connection = DatabaseConnection.Instance.GetConnection())
                {
                    connection.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand("spAddClient", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FullName", client.FullName);
                        cmd.Parameters.AddWithValue("@Phone", client.Phone);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Клиент {client.FullName} добавлен");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    connection.Close();
                }
            }
            else
                MessageBox.Show("Введите имя клиента");
        }

        public void AddRental(Rental rental)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spAddRental", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientID", rental.Client.Id);
                    for (int i = 0; i < rental.Equipments.Count; i++)
                        if (rental.Equipments[i] != null)
                            cmd.Parameters.AddWithValue($"@Equip{i + 1}ID", rental.Equipments[i].Id);
                    cmd.Parameters.AddWithValue("@DateBegin", rental.DateBegin);
                    cmd.Parameters.AddWithValue("@DateEnd", rental.DateEnd);
                    cmd.Parameters.AddWithValue("Payment", rental.Payed.ToString("F2", new CultureInfo("en-US")));
                    SqlParameter errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 100);
                    errorMessageParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(errorMessageParam);
                    cmd.ExecuteNonQuery();
                    if (!string.IsNullOrEmpty(errorMessageParam.Value as string))
                        MessageBox.Show(errorMessageParam.Value as string);
                    else
                        MessageBox.Show("Аренда успешно зарегистрирована!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }

        public void AddPayment(Payment payment)
        {
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("spAddPayment", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RentalID", payment.RentalId);
                    cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                    cmd.Parameters.AddWithValue("Amount", payment.Amount);
                    SqlParameter errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 100);
                    errorMessageParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(errorMessageParam);
                    cmd.ExecuteNonQuery();
                    if (!string.IsNullOrEmpty(errorMessageParam.Value as string))
                        MessageBox.Show(errorMessageParam.Value as string);
                    else
                        MessageBox.Show($"Платеж на сумму {payment.Amount} совершен!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }
        #endregion

        #region ANSWERQUERYMETHODS
        public QueryA AnswerQueryA(Category category)
        {
            QueryA queryA = null;
            using(var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spCourseQueryA", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Category", category.Title);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    queryA = new QueryA
                    {
                        Category = (string)reader["Category"],
                        ClientName = (string)reader["ClientName"],
                        TotalSum = (decimal)reader["TotalSum"]
                    };
                reader.Close();
                connection.Close();
            }
            return queryA;
        }

        public List<QueryB> AnswerQueryB()
        {
            List <QueryB> queryB = new List <QueryB>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spCourseQueryB", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    queryB.Add(new QueryB
                    {
                        TopCategory = (string)reader["TopCategory"],
                        EquipmentTitle = (string)reader["Equipment"],
                        RentalCount = (int)reader["RentalCount"]
                    });
                reader.Close();
                connection.Close();
            }
            return queryB;
        }

        public List<QueryC> AnswerQueryC()
        {
            List<QueryC> queryC = new List<QueryC>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spCourseQueryC", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    queryC.Add(new QueryC
                    {
                        ClientName = (string)reader["ClientName"],
                        ClientPhone = (string)reader["ClientPhone"]
                    });
                reader.Close();
                connection.Close();
            }
            return queryC;
        }

        public List<QueryD> AnswerQueryD(DateTime dateBegin, DateTime dateEnd)
        {
            List<QueryD> queryD = new List<QueryD>();
            using (var connection = DatabaseConnection.Instance.GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spCourseQueryD", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateBegin", dateBegin);
                cmd.Parameters.AddWithValue("@DateEnd", dateEnd);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    queryD.Add(new QueryD
                    {
                        EquipmentsString = GetEquipmentsTitleStringByEquiomentsList(GetEquipmentsByRentalID((int)reader["RentalID"])),
                        ClientName = (string)reader["FullName"],
                        ClientPhone = (string)reader["Phone"],
                        SumPrice = (decimal)reader["SumPrice"],
                        DateBegin = (DateTime)reader["DateBegin"],
                        DateEnd = (DateTime)reader["DateEnd"],
                        Payed = (decimal)reader["Payed"],
                        Total = (decimal)reader["Total"],
                        ToPay = (decimal)reader["ToPay"]
                    });
                reader.Close();
                connection.Close();
            }
            return queryD;
        }
        #endregion
    }
}
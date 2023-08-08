using FAP_API.Models;
using System.Data.SQLite;

namespace FAP_API.DatabaseClasses
{
    public class UserDbManager : DbManager
    {
        public UserDbManager(string connectionString) : base(connectionString) { }

        #region USERS
        public List<User> GetUsers()
        {
            List<User> listUsers = new List<User>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM user", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listUsers.Add(CreateUserObject(reader));
                        }
                    }
                }
            }

            return listUsers;

        }

        private User CreateUserObject(SQLiteDataReader reader)
        {
            return new User
            {
                Id = Convert.ToInt32(reader["id"]),
                Username = Convert.ToString(reader["username"]),
                Firstname = Convert.ToString(reader["firstname"]),
                Lastname = Convert.ToString(reader["Lastname"]),
                Email = Convert.ToString(reader["email"]),
                Phone = Convert.ToString(reader["phone"]),
                Adress = Convert.ToString(reader["adress"]),
                City = Convert.ToString(reader["city"]),
                Country = Convert.ToString(reader["country"]),
                Password = Convert.ToString(reader["password"]),
            };
        }

        public int AddUser(User user)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("INSERT INTO user (username, firstname, lastname, email, phone, adress, city, country, password) VALUES (@Username, @Firstname, @Lastname, @Email, @Phone, @Adress, @City, @Country, @Password); SELECT last_insert_rowid();", conn))
                {
                    //command.Parameters.AddWithValue("@Id", school.Id);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Firstname", user.Firstname);
                    command.Parameters.AddWithValue("@Lastname", user.Lastname);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Phone", user.Phone);
                    command.Parameters.AddWithValue("@Adress", user.Adress);
                    command.Parameters.AddWithValue("@City", user.City);
                    command.Parameters.AddWithValue("@Country", user.Country);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new User ID.");
                    }
                }
            }
        }

        public User GetUserById(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("SELECT * FROM user WHERE id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateUserObject(reader);
                        }
                    }
                }
            }

            return null;
        }

		/*public List<User> GetUserByFirstname(string firstname)
		{
			List<User> listUsers = new List<User>();

			using (var conn = new SQLiteConnection(connectionString))
			{
				conn.Open();

				using (var command = new SQLiteCommand("SELECT * FROM user WHERE firstname=@Firstname ", conn))
				{
					command.Parameters.AddWithValue("@Firstname", firstname);

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							listUsers.Add(CreateUserObject(reader));
						}
					}
				}
			}

			return listUsers;
		}*/

		public User ExistanceUser(UserDto user)
        {
            User us = null;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM user", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string username = Convert.ToString(reader["username"]);
                            string firstname = Convert.ToString(reader["firstname"]);
                            string lastname = Convert.ToString(reader["lastname"]);
                            string email = Convert.ToString(reader["email"]);
                            string phone = Convert.ToString(reader["phone"]);
                            string adress = Convert.ToString(reader["adress"]);
                            string city = Convert.ToString(reader["city"]);
                            string country = Convert.ToString(reader["country"]);
                            string password = Convert.ToString(reader["password"]);
                            if (username == user.Username && password == user.Password)
                            {
                                User u = new User();
                                u.Id = id;
                                u.Username = username;
                                u.Firstname = firstname;
                                u.Lastname = lastname;
                                u.Email = email;
                                u.Phone = phone;
                                u.Adress = adress;
                                u.City = city;
                                u.Country = country;
                                u.Password = password;
                                return u;
                            }
                        }
                    }
                }
            }
            return us;
        }

        public bool UpdateUser(User user)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("UPDATE user SET username=@Username, firstname=@Firstname, lastname=@Lastname, email=@Email, phone=@Phone, adress=@Adress, city=@City, country=@Country, password=@Password WHERE id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Firstname", user.Firstname);
                    command.Parameters.AddWithValue("@Lastname", user.Lastname);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("Phone", user.Phone);
                    command.Parameters.AddWithValue("@Adress", user.Adress);
                    command.Parameters.AddWithValue("@City", user.City);
                    command.Parameters.AddWithValue("@Country", user.Country);
                    command.Parameters.AddWithValue("@Password", GetUserById(user.Id).Password);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        public bool DeleteUser(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("DELETE FROM user WHERE id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }
        #endregion
    }
}

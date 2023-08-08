using FAP_API.Models;
using System.Data.SQLite;

namespace FAP_API.DatabaseClasses
{
    public class MessageDbManager: DbManager
    {
        public MessageDbManager(string connectionString) : base(connectionString) { }
        #region MESSAGE
        public List<Message> GetAllMessage(int id, int code)
        {
            List<Message> listMessage = new List<Message>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM message", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int exp_id = Convert.ToInt32(reader["expeditious_id"]);
                            int ben_id = Convert.ToInt32(reader["beneficiary_id"]);
                            if ((exp_id == id && ben_id == code) || (ben_id == id && exp_id == code))
                            {
                                listMessage.Add(CreateMessageObject(reader));
                            }
                        }
                    }
                }
            }
            return listMessage;
        }

        public List<Message> GetAllMessageUser(int id)
        {
            List<Message> listMessage = new List<Message>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM message GROUP BY expeditious_id, beneficiary_id", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int exp_id = Convert.ToInt32(reader["expeditious_id"]);
                            int ben_id = Convert.ToInt32(reader["beneficiary_id"]);
                            if (exp_id == id || ben_id == id)
                            {
                                listMessage.Add(CreateMessageObject(reader));
                            }
                        }
                    }
                }
            }
            return listMessage;
        }

        public List<Message> GetAllMesUser(int id)
        {
            List<Message> listMessage = new List<Message>();
            List<Message> messages = new List<Message>();
            foreach (Message message in GetAllMessageUser(id))
            {
                if (listMessage.Count() == 0)
                {
                    listMessage.Add(message);
                }
                else
                {
                    foreach (Message mes in listMessage)
                    {
                        if (message.ExpeditiousID == mes.ExpeditiousID && message.BeneficiaryID == mes.BeneficiaryID)
                        {

                        }
                        else if (message.ExpeditiousID == mes.BeneficiaryID && message.BeneficiaryID == mes.ExpeditiousID)
                        {

                        }
                        else
                        {
                            messages.Add(message);
                        }
                    }
                }
            }
            return listMessage;
        }

        private Message CreateMessageObject(SQLiteDataReader reader)
        {
            return new Message
            {
                Id = Convert.ToInt32(reader["id"]),
                Content = Convert.ToString(reader["content"]),
                ExpeditiousID = Convert.ToInt32(reader["expeditious_id"]),
                BeneficiaryID = Convert.ToInt32(reader["beneficiary_id"]),
            };
        }

        public int AddMessage(Message message)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("INSERT INTO message (content, expeditious_id, beneficiary_id) VALUES (@Content, @ExpeditiousID, @BeneficiaryID); SELECT last_insert_rowid();", conn))
                {
                    //command.Parameters.AddWithValue("@Id", school.Id);
                    command.Parameters.AddWithValue("@Content", message.Content);
                   
                    command.Parameters.AddWithValue("@ExpeditiousID", message.ExpeditiousID);
                    command.Parameters.AddWithValue("@BeneficiaryID", message.BeneficiaryID);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new Message ID.");
                    }
                }
            }
        }

        public Message GetMessageById(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("SELECT * FROM message WHERE id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateMessageObject(reader);
                        }
                    }
                }
            }

            return null;
        }

        public Message GetExpMessage(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("SELECT * FROM message WHERE expeditious_id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateMessageObject(reader);
                        }
                    }
                }
            }

            return null;
        }

        public Message GetBenMessage(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("SELECT * FROM message WHERE beneficiary_id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateMessageObject(reader);
                        }
                    }
                }
            }

            return null;
        }

        public bool DeleteMessage(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("DELETE FROM message WHERE id=@id", conn))
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

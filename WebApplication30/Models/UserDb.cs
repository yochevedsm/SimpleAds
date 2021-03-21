using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication30.Models
{
    public class UserDb
    {
        private readonly string _connectionString;

        public UserDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(Users user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Name, Email, PasswordHash) " +
                              "VALUES (@name, @email, @passwordHash)";
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
            connection.Open();
            cmd.ExecuteNonQuery();

        }
        public Users Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;
        }


            public Users GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new Users
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }


        public List<Ads> GetAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT A.*, u.Name From Ads A JOIN USERS U ON A.UserId = U.Id ORDER BY Date DESC";
            connection.Open();
            List<Ads> ads = new List<Ads>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(new Ads
                {
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Details = (string)reader["Details"],
                    Date = (DateTime)reader["Date"],
                    UserName = (string)reader["Name"]

                });


                    
            }

            return ads;
        }

        public void NewAd(Ads ads)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Ads VALUES( @userId, @phoneNumber, @details, @date )";
            
            cmd.Parameters.AddWithValue("@userId", ads.UserId);
            cmd.Parameters.AddWithValue("@phoneNumber", ads.PhoneNumber);
            cmd.Parameters.AddWithValue("@details", ads.Details);
            cmd.Parameters.AddWithValue("@date", ads.Date);

            connection.Open();
            cmd.ExecuteNonQuery();

            
        }

        public void DeleteAd(int id)
        {
            var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE from Ads WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }


}


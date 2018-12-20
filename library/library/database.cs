using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace library
{
    class database
    {

        private static string connection = @"Data Source=EUGENE;Initial Catalog=books;Integrated Security=True;Pooling=False"; //Подключение к базе

        SqlConnection cnn = new SqlConnection(connection); 
      
        public void addToDB(string bookName, string bookLink) { //Добавление в базу
            cnn.Open();
            
            string queryDB = "INSERT INTO [Books] (bookName, bookLink)VALUES(@bookName, @bookLink);"; //Sql
            SqlCommand cmd = new SqlCommand(queryDB, cnn);

            cmd.Parameters.AddWithValue("bookName", bookName);
            cmd.Parameters.AddWithValue("bookLink", bookLink);

            cmd.ExecuteNonQuery();

            cnn.Close();

        }

        public void openFile(string bookName) {
            cnn.Open();
            string queryDB = "SELECT bookLink, bookName FROM [Books] WHERE bookName='"+ bookName + "'";
            SqlCommand cmd = new SqlCommand(queryDB, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            string response = string.Empty;

            while (reader.Read()) {
                response += reader["bookLink"];
            }

            cnn.Close();

            System.Diagnostics.Process.Start(response);
        }

        public void delFromDB(string bookName) {
            
            string queryDB = "DELETE FROM [Books] WHERE bookName='"+ bookName +"'";
            cnn.Open();
            SqlCommand cmd = new SqlCommand(queryDB, cnn);
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}

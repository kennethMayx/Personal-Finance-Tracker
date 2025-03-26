
using System;
using System.Collections.Generic;
using System.Data.SQLite;

public class DatabaseHelper
{
    private readonly string _connectionString = "Data Source=finance.db;Version=3;";

    public DatabaseHelper()
    {
        CreateTables();
    }

    private void CreateTables()
    {
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();

            string userTable = @"CREATE TABLE IF NOT EXISTS Users (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    name TEXT NOT NULL,
                                    email TEXT NOT NULL,
                                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                                 );";

            string transactionTable = @"CREATE TABLE IF NOT EXISTS Transactions (
                                           id INTEGER PRIMARY KEY AUTOINCREMENT,
                                           user_id INTEGER,
                                           type TEXT,
                                           category TEXT,
                                           amount REAL,
                                           date DATETIME,
                                           description TEXT,
                                           FOREIGN KEY(user_id) REFERENCES Users(id)
                                       );";

            using (var cmd = new SQLiteCommand(userTable, conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new SQLiteCommand(transactionTable, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void SaveTransaction(Transaction transaction)
    {
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string insert = @"INSERT INTO Transactions (user_id, type, category, amount, date, description)
                              VALUES (@user_id, @type, @category, @amount, @date, @description);";

            using (var cmd = new SQLiteCommand(insert, conn))
            {
                cmd.Parameters.AddWithValue("@user_id", transaction.UserId);
                cmd.Parameters.AddWithValue("@type", transaction is Income ? "Income" : "Expense");
                cmd.Parameters.AddWithValue("@category", "General"); // Add category logic if needed
                cmd.Parameters.AddWithValue("@amount", transaction.Amount);
                cmd.Parameters.AddWithValue("@date", transaction.Date);
                cmd.Parameters.AddWithValue("@description", transaction.Description);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<Transaction> GetTransactionsByUserId(int userId)
    {
        List<Transaction> transactions = new List<Transaction>();

        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string select = "SELECT * FROM Transactions WHERE user_id = @user_id;";

            using (var cmd = new SQLiteCommand(select, conn))
            {
                cmd.Parameters.AddWithValue("@user_id", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string type = reader["type"].ToString();
                        Transaction transaction = type == "Income" ? new Income() : (Transaction)new Expense();

                        transaction.Id = Convert.ToInt32(reader["id"]);
                        transaction.UserId = Convert.ToInt32(reader["user_id"]);
                        transaction.Amount = Convert.ToDouble(reader["amount"]);
                        transaction.Date = Convert.ToDateTime(reader["date"]);
                        transaction.Description = reader["description"].ToString();

                        transactions.Add(transaction);
                    }
                }
            }
        }

        return transactions;
    }

    public void DeleteTransactionById(int id)
    {
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string delete = "DELETE FROM Transactions WHERE id = @id;";
            using (var cmd = new SQLiteCommand(delete, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

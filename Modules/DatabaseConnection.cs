using Microsoft.Data.Sqlite;

namespace LyxBot.Modules
{
    public static class DatabaseConnection
    {
        private const string ConnectionString = "Data Source=LyxBot.db";

        public static async Task UpdateUserBalance(string userId, int amount)
        {
            await using SqliteConnection connection = new(ConnectionString);
            connection.Open();

            // Check if user exists
            string selectQuery = "SELECT Balance FROM UserBalances WHERE UserId = @UserId";
            await using SqliteCommand selectCommand = new(selectQuery, connection);
            selectCommand.Parameters.AddWithValue("@UserId", userId);
            object? result = selectCommand.ExecuteScalar();

            if (result == null)
            {
                // Add the user if it doesn't exist already in the DB
                string insertQuery = "INSERT INTO UserBalances (UserId, Balance) VALUES (@UserId, @Balance)";
                await using SqliteCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@UserId", userId);
                insertCommand.Parameters.AddWithValue("@Balance", amount);
                await insertCommand.ExecuteNonQueryAsync();
            }
            else
            {
                // Update user balance if user already exists
                int currentBalance = Convert.ToInt32(result);
                int newBalance = currentBalance + amount;

                if (newBalance < 0 || currentBalance < amount)
                {
                    throw new InvalidOperationException("You don't have enough Lyx coins!");
                }

                string updateQuery = "UPDATE UserBalances SET Balance = @Balance WHERE UserId = @UserId";
                await using SqliteCommand updateCommand = new(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@Balance", newBalance);
                updateCommand.Parameters.AddWithValue("@UserId", userId);
                await updateCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
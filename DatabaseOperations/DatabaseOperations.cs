using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace RecipeManager.DatabaseOperations
{
    public class DatabaseOperations
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=password;Database=recipemanager";

        public async Task CreateTablesAsync()
        {
            try
            {
                var statements = new List<string>
                {
                    @"
                    CREATE TABLE IF NOT EXISTS ingredients (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL UNIQUE
                    )",
                    @"
                    CREATE TABLE IF NOT EXISTS recipes(
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL,
                        Description TEXT
                    )",
                    @"
                    CREATE TABLE IF NOT EXISTS recipeingredients (
                        RecipeId INT NOT NULL,
                        IngredientId INT NOT NULL,
                        PRIMARY KEY (RecipeId, IngredientId),
                        FOREIGN KEY (RecipeId) REFERENCES recipes(Id) ON DELETE CASCADE,
                        FOREIGN KEY (IngredientId) REFERENCES ingredients(Id) ON DELETE CASCADE
                    )"
                };

                await using var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();

                foreach (var statement in statements)
                {
                    await using var cmd = new NpgsqlCommand(statement, conn);
                    await cmd.ExecuteNonQueryAsync();
                }

                Console.WriteLine("The tables have been created successfully.");
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

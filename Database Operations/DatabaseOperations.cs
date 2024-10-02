using System;
using System.Collections.Genric;
using System.Threading.Tasks;
using Npgsql;

namespace RecipeManager.DatabaseOperations
{
    public class DatabaseOperations
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=password;Database=recipemanager";

        pubilc async async Task CreateTablesAsync()
        {
            try
            {
                var statments = new List<string>
                {
                    @"
                    CREATE TABLE IF NOT EXISTS ingredients (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(255) NOT NULL UNIQUE,
                        )",
                    @"
                    CREATE TABLE IF NOT EXISTS recipes(
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(255) NOT NULL),
                        Description NVARCHAR(MAX)
                        )",
                    @"
                    CREATE TABLE IF NOT EXISTS RecipeIngredients (
                        RecipeId INT NOT NULL,
                        IngredientId INT NOT NULL,
                        PRIMARY KEY (RecipeId, IngredientId),
                        FOREIGN KEY (RecipeId) REFERENCES Recipes(Id) ON DELETE CASCADE,
                        FOREIGN KEY (IngredientId) REFERENCES Ingredients(Id) ON DELETE CASCADE
                    )"
                };

                await using var dataSource = NpgsqlDataSource.Create(connectionString);

                foreach (var statement in statements)
                {
                    await using var cmd = dataSource.CreateCommmand(statement);
                    await cmd.ExecuteNonQueryAsync();
                }

                Console.WriteLine("The tables have been creaed successfully.");
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
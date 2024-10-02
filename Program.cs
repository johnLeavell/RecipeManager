using Microsoft.Extensions.DependencyInjection;
using RecipeManager.Data;
using RecipeManager.Repositories;
using RecipeManager.Services;
using Spectre.Console;
using RecipeManager.Helpers;
using RecipeManager.DatabaseOperations;

namespace RecipeManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Build configuration
            var serviceProvider = new ServiceCollection()
                .AddDbContext<RecipeContext>()
                .AddScoped(typeof(GenericRepository<>))
                .AddScoped<RecipeService>()
                .AddTransient<RecipeManager.DatabaseOperations.DatabaseOperations>() // Ensure DatabaseOperations is added here
                .BuildServiceProvider();

            // Use the services
            var dbOps = serviceProvider.GetService<RecipeManager.DatabaseOperations.DatabaseOperations>();
            if (dbOps != null)
            {
                await dbOps.CreateTablesAsync();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to create DatabaseOperations service.[/]");
                return;
            }

            var recipeService = serviceProvider.GetService<RecipeService>();

            // Command line interface loop
            while (true)
            {
                ShowMainMenuHeading();
                ShowMainMenuOptions();

                var choice = Console.ReadLine() ?? string.Empty;
                if (InputHelper.CheckForSpecialCommands(choice))
                {
                    continue;
                }

                switch (choice)
                {
                    case "1":
                        var recipes = await recipeService.GetAllRecipesAsync();
                        foreach (var recipe in recipes)
                        {
                            Console.WriteLine($"{recipe.Id}: {recipe.Name} - {recipe.Description}");
                        }
                        break;
                        // Implement other cases for Add, Update, Delete, Filter, and Sort.
                }
            }
        }

        private static void ShowMainMenuHeading()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("Recipe Manager")
                .Centered()
                .Color(Color.Green));
        }

        private static void ShowMainMenuOptions()
        {
            AnsiConsole.MarkupLine("Choose an option:");
            AnsiConsole.MarkupLine("1. [green]List Recipes[/]");
            AnsiConsole.MarkupLine("2. [green]Add Recipe[/]");
            AnsiConsole.MarkupLine("3. [green]Update Recipe[/]");
            AnsiConsole.MarkupLine("4. [green]Delete Recipe[/]");
            AnsiConsole.MarkupLine("5. [green]Filter Recipes[/]");
            AnsiConsole.MarkupLine("6. [green]Sort Recipes[/]");
            AnsiConsole.MarkupLine("Option: ");
        }
    }
}

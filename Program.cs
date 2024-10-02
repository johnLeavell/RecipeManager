using Microsoft.Extensions.DependencyInjection;
using RecipeManager.Data;
using RecipeManager.Repositories;
using RecipeManager.Services;
using Spectre.Console;
using RecipeManager.Helpers;

namespace RecipeManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // build configuration
            var serviceProvider = new ServiceCollection()
                .AddDbContext<RecipeContext>()
                .AddScoped(typeof(GenericRepository<>))
                .AddScoped<RecipeService>()
                .BuildServiceProvider();

            var recipeService = serviceProvider.GetService<RecipeService>();

            //command line interface loop
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

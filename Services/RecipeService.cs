// Services/RecipeService.cs
using RecipeManager.Models;
using RecipeManager.Repositories;

namespace RecipeManager.Services
{
    public class RecipeService
    {
        private readonly GenericRepository<Recipe> _recipeRepository;

        public RecipeService(GenericRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync(string? filter = null, string? sortBy = null)
        {
            var recipes = await _recipeRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(filter))
            {
                recipes = recipes.Where(r => r.Name.Contains(filter));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                recipes = sortBy.ToLower() switch
                {
                    "name" => recipes.OrderBy(r => r.Name),
                    "description" => recipes.OrderBy(r => r.Description),
                    _ => recipes
                };
            }

            return recipes;
        }
    }
}

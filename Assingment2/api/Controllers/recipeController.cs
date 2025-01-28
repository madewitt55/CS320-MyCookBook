using Microsoft.AspNetCore.Mvc;
using MyCookBookApi.Models;
[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase {
[HttpGet]
    public IActionResult GetRecipes() {
    return Ok(new List<Recipe> {
        new Recipe { name = "Pasta", 
        ingredients = new List<string> { "Pasta", "Tomato Sauce" }, 
        steps = "Boil pasta." },
        new Recipe { name = "Salad", 
        ingredients = new List<string> { "Lettuce", "Tomatoes" }, 
        steps = "Mix all ingredients." }
    });
    }
}
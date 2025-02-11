using Microsoft.AspNetCore.Mvc;
using MyCookBookApi.Models;
[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
private static readonly List<Recipe> Recipes = new List<Recipe>
{
new Recipe { name = "Pasta", ingredients = new List<string> { "Pasta",
"Tomato Sauce" }, steps = "Boil pasta and mix with sauce." },
new Recipe { name = "Salad", ingredients = new List<string> { "Lettuce",
"Tomatoes", "Cucumbers" }, steps = "Chop and mix ingredients." }
};
[HttpGet]
public IActionResult GetRecipes()
{
return Ok(Recipes);
}
[HttpPost("search")]
public IActionResult Search([FromBody] RecipeSearchRequest request)
{
if (string.IsNullOrWhiteSpace(request.query))
{
return BadRequest("query cannot be empty.");
}
var results = Recipes
.Where(r => r.name.Contains(request.query,
System.StringComparison.OrdinalIgnoreCase))
.ToList();
return Ok(results);
}
}
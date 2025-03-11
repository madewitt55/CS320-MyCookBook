using Xunit;
using MyCookBookApp.Models; // Namespace for the Recipe model
using System.Collections.Generic;
namespace MyCookBookApp.Tests
{
 public class RecipeModelTests
 {
 [Fact]
 public void RecipeModel_ShouldStoreDataCorrectly()
 {
 // Arrange
 var recipe = new Recipe
 {
 name = "Salad",
 ingredients = new List<string> { "Lettuce",
"Tomatoes", "Dressing" },
 steps = "Mix ingredients together."
 };
 // Assert
 Assert.Equal("Salad", recipe.name);
 Assert.Contains("Tomatoes", recipe.ingredients);
 Assert.Equal("Mix ingredients together.", recipe.steps);
 }
 }
}
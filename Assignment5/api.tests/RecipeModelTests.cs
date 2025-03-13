using Xunit;
using MyCookBookApi.Models; // Namespace for the Recipe model
using System.Collections.Generic;
namespace MyCookBookApi.Tests
{
 public class RecipeModelTests
 {
 [Fact]
 public void RecipeModel_ShouldStoreDataCorrectly()
 {
 // Arrange
 var recipe = new Recipe
 {
 name = "Pasta", ingredients = new List<string> { "Pasta", "Tomato Sauce" },
 steps = "Boil pasta."
 };
 // Assert
 Assert.Equal("Pasta", recipe.name);
 Assert.Contains("Tomato Sauce", recipe.ingredients);
 Assert.Equal("Boil pasta.", recipe.steps);
 }
 }
}
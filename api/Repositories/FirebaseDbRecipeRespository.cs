using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using MyCookBookApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace MyCookBookApi.Repositories
{
public class FirebaseDbRecipeRepository : IRecipeRepository
{
private readonly FirestoreDb _firestoreDb;
private const string CollectionName = "Recipes";
public FirebaseDbRecipeRepository()
{
string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot",
"YourServiceAccountKey.json");
if (!File.Exists(path))
{
throw new FileNotFoundException("Firebase service account key not found.", path);
}

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
_firestoreDb = FirestoreDb.Create("mycookbook-db");
}
public List<Recipe> GetAllRecipes()
{
return GetAllRecipesAsync().GetAwaiter().GetResult();
}
public Recipe GetRecipeById(string id)
{
return GetRecipeByIdAsync(id).GetAwaiter().GetResult();
}
public List<Recipe> SearchRecipes(RecipeSearchRequest searchRequest)
{
return SearchRecipesAsync(searchRequest).GetAwaiter().GetResult();
}
public void AddRecipe(Recipe recipe)
{
AddRecipeAsync(recipe).GetAwaiter().GetResult();
}
public bool UpdateRecipe(string id, Recipe updatedRecipe)
{
return UpdateRecipeAsync(id, updatedRecipe).GetAwaiter().GetResult();
}
public bool DeleteRecipe(string id)
{
return DeleteRecipeAsync(id).GetAwaiter().GetResult();
}
// 🔹 Get All Recipes
private async Task<List<Recipe>> GetAllRecipesAsync()
{
Query query = _firestoreDb.Collection(CollectionName);
QuerySnapshot snapshot = await query.GetSnapshotAsync();
return snapshot.Documents
.Select(doc => doc.ConvertTo<Recipe>())
.ToList();
}
// 🔹 Get Recipe by ID
private async Task<Recipe> GetRecipeByIdAsync(string id)
{
DocumentSnapshot snapshot = await
_firestoreDb.Collection(CollectionName).Document(id).GetSnapshotAsync();
return snapshot.Exists ? snapshot.ConvertTo<Recipe>() : null;
}
// 🔹 Search Recipes (Firestore lacks case-insensitive queries, so filtering is in-memory)
private async Task<List<Recipe>> SearchRecipesAsync(RecipeSearchRequest searchRequest)
{
Query query = _firestoreDb.Collection(CollectionName);
QuerySnapshot snapshot = await query.GetSnapshotAsync();
return snapshot.Documents
.Select(doc => doc.ConvertTo<Recipe>())
.Where(r => string.IsNullOrEmpty(searchRequest.Keyword) ||
r.Name.Contains(searchRequest.Keyword,
StringComparison.OrdinalIgnoreCase) ||
r.Summary.Contains(searchRequest.Keyword,
StringComparison.OrdinalIgnoreCase))
.ToList();
}

// 🔹 Add Recipe
private async Task AddRecipeAsync(Recipe recipe)
{
if (recipe == null || string.IsNullOrWhiteSpace(recipe.RecipeId))
{
throw new ArgumentException("Invalid recipe data: Recipe ID cannot be null.");
}
DocumentReference docRef =
_firestoreDb.Collection(CollectionName).Document(recipe.RecipeId);
await docRef.SetAsync(recipe);
}
// 🔹 Update Recipe
private async Task<bool> UpdateRecipeAsync(string id, Recipe updatedRecipe)
{
if (updatedRecipe == null) return false;
DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
if (!snapshot.Exists) return false;
Dictionary<string, object> updateData = new()
{
{ "Name", updatedRecipe.Name },
{ "TagLine", updatedRecipe.TagLine },
{ "Summary", updatedRecipe.Summary },
{ "Ingredients", updatedRecipe.Ingredients ?? new List<string>() },
{ "Instructions", updatedRecipe.Instructions ?? new List<string>() },
{ "Categories", updatedRecipe.Categories?.Select(c => c.ToString()).ToList() ??
new List<string>() },
{ "Media", updatedRecipe.Media != null
? updatedRecipe.Media.Select(m => new Dictionary<string, object>
{
{ "Url", m.Url },
{ "Type", m.Type },
{ "Order", m.Order }
}).ToList()
: new List<Dictionary<string, object>>() }
};
await docRef.UpdateAsync(updateData);
return true;
}
// 🔹 Delete Recipe
private async Task<bool> DeleteRecipeAsync(string id)
{
DocumentReference docRef = _firestoreDb.Collection(CollectionName).Document(id);
DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
if (!snapshot.Exists) return false;
await docRef.DeleteAsync();
return true;
}
}
}
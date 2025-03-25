using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Linq;
namespace MyCookBookApi.Models
{
public class RecipeMediaConverter : IFirestoreConverter<List<RecipeMedia>>
{
public object ToFirestore(List<RecipeMedia> mediaList)
{
return mediaList.Select(media => new Dictionary<string, object>
{
{ "Url", media.Url },
{ "Type", media.Type },
{ "Order", media.Order }
}).ToList();
}
public List<RecipeMedia> FromFirestore(object value)
{
if (value is List<object> list)
{
return list
.OfType<Dictionary<string, object>>() // Ensure conversion safety
.Select(dict => new RecipeMedia
{
Url = dict.ContainsKey("Url") ? dict["Url"].ToString() : "",
Type = dict.ContainsKey("Type") ? dict["Type"].ToString() :
"",
Order = dict.ContainsKey("Order") ?
Convert.ToInt32(dict["Order"]) : 0
}).ToList();
}
return new List<RecipeMedia>(); // Return empty list if conversion fails
}
}
}
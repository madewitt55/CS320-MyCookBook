using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyCookBookApp.Models;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
namespace MyCookBookApp.Services {
    public class RecipeService {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public RecipeService(HttpClient httpClient, IConfiguration configuration) {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }
        public async Task<List<Recipe>> GetRecipesAsync() {
            var response = await _httpClient.GetAsync($"{_baseUrl}/recipe");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Recipe>>(json);
        }
        public async Task<Recipe> GetRecipeByIdAsync(string id) {
            var response = await _httpClient.GetAsync($"{_baseUrl}/recipe/{id}");
            if (!response.IsSuccessStatusCode)
            return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Recipe>(json);
        }
        public async Task<List<Recipe>> SearchRecipesAsync(RecipeSearchRequest
        searchRequest) {
            var content = new
            StringContent(JsonConvert.SerializeObject(searchRequest), Encoding.UTF8,
            "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/recipe/search",
            content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Recipe>>(json);
        }

        public async Task<bool> AddRecipeAsync(Recipe recipe) {
            var json = JsonConvert.SerializeObject(recipe, Formatting.Indented); 
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // ✅ Log the request body before sending
            Console.WriteLine("Request Body:");
            Console.WriteLine(json);
            // var content = new StringContent(JsonConvert.SerializeObject(recipe),
            //Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/recipe", content);
            return response.IsSuccessStatusCode;
        }
    }
}
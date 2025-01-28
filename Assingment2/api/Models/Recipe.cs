namespace MyCookBookApi.Models
{
    public class Recipe {
        public string name { get; set; }
        public List<string> ingredients { get; set; }
        public string steps { get; set; }
    }
}
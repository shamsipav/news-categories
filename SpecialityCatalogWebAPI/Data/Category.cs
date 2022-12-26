namespace SpecialityCatalogWebAPI.Data
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<News> News { get; set; } = new();
    }
}

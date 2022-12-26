using System.ComponentModel.DataAnnotations.Schema;

namespace SpecialityCatalogWebAPI.Data
{
    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }

        public List<Category> Categories { get; set; } = new();
    }
}

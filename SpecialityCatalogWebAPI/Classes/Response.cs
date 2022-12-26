using SpecialityCatalogWebAPI.Data;

namespace SpecialityCatalogWebAPI.Classes
{
    public class Response
    {
        public bool Ok { get; set; }
        public News? News { get; set; }
        public Category? Category { get; set; }
        public string Message { get; set; }
    }
}

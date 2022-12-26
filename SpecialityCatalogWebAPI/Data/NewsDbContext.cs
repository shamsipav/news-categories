using Microsoft.EntityFrameworkCore;

namespace SpecialityCatalogWebAPI.Data
{
    public class NewsDbContext: DbContext
    {
        public DbSet<Category> Categories  { get; set; }
        public DbSet<News> News { get; set; }

        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) 
        {
            
        }
    }
}

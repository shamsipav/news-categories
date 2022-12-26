using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpecialityCatalogWebAPI.Classes;
using SpecialityCatalogWebAPI.Data;

namespace SpecialityCatalogWebAPI.Controllers
{
    [Route("api/[controller]")]
    /*[Authorize]*/
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly NewsDbContext _newsDbContext;
        public CategoryController(NewsDbContext newsDbContext)
        {
            _newsDbContext = newsDbContext;
        }

        [HttpGet]
        public List<Category> Get()
        {
            var categories = _newsDbContext.Categories
                .Include(x => x.News)
                .ToList();

            return categories;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _newsDbContext.Categories
                .Include(x => x.News)
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public IActionResult Post(Category category)
        {
            _newsDbContext.Categories.Add(category);
            _newsDbContext.SaveChanges();

            return Ok(new Response { Ok = true, Category = category, Message = "Категория успешно добавлена" });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Category category)
        {
            var existCategory = _newsDbContext.Categories.Include(x => x.News).FirstOrDefault(x => x.Id == id);

            if (existCategory == null)
            {
                return NotFound();
            }

            existCategory.Name = category.Name;
            _newsDbContext.Categories.Update(existCategory);
            _newsDbContext.SaveChanges();

            return Ok(new Response { Ok = true, Category = existCategory, Message = "Категория успешно обновлена" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existCategory = _newsDbContext.Categories.FirstOrDefault(x => x.Id == id);

            if (existCategory == null)
            {
                return NotFound();
            }

            _newsDbContext.Categories.Remove(existCategory);
            _newsDbContext.SaveChanges();

            return Ok(new Response { Ok = true, Category = existCategory, Message = "Категория успешно удалена" });
        }
    }
}

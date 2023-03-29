using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpecialityCatalogWebAPI.Classes;
using SpecialityCatalogWebAPI.Data;

namespace SpecialityCatalogWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsDbContext _newsDbContext;
        public NewsController(NewsDbContext newsDbContext)
        {
            _newsDbContext = newsDbContext;
        }

        [HttpGet]
        public List<News> Get()
        {
            var newsList = _newsDbContext.News
                .Include(x => x.Categories)
                .ToList();

            return newsList;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var news = _newsDbContext.News
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(News news)
        {
            List<Category> categories = new List<Category>();

            foreach (var category in news.Categories)
            {
                Category? existCategory = _newsDbContext.Categories.FirstOrDefault(x => x.Id == category.Id);
                if (existCategory != null)
                {
                    categories.Add(existCategory);
                }
            }

            news.Categories.Clear();
            news.Categories = categories;

            news.CreateTime = DateTime.Now;

            _newsDbContext.News.Add(news);
            _newsDbContext.SaveChanges();

            return Ok(new Response { Ok = true, News = news, Message = "Новость успешно добавлена" });
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, News news)
        {
            var existNews = _newsDbContext.News.Include(x => x.Categories).FirstOrDefault(x => x.Id == id);

            if (existNews == null)
            {
                return NotFound();
            }

            existNews.Title = news.Title;
            existNews.Text = news.Text;
            existNews.CreateTime = news.CreateTime;

            List<Category> categories = new List<Category>();

            foreach (var category in news.Categories)
            {
                Category? existCategory = _newsDbContext.Categories.FirstOrDefault(x => x.Id == category.Id);
                if (existCategory != null)
                {
                    categories.Add(existCategory);
                }
            }

            existNews.Categories.Clear();
            existNews.Categories = categories;

            _newsDbContext.News.Update(existNews);
            _newsDbContext.SaveChanges();

            return Ok(new Response { Ok = true, News = existNews, Message = "Новость успешно обновлена" });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existNews = _newsDbContext.News.FirstOrDefault(x => x.Id == id);

            if (existNews == null)
            {
                return NotFound();
            }

            _newsDbContext.News.Remove(existNews);
            _newsDbContext.SaveChanges();

            return Ok(new Response { Ok = true, News = existNews, Message = $"Новость '{existNews.Title}' успешно удалена" });
        }
    }
}

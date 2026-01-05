using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YaroshenkoShop.Models;
using System.Linq;
using System.Threading.Tasks;
using YaroshenkoShop.Data;

namespace YaroshenkoShop.Controllers
{
    public class CatalogController : Controller
    {
        private readonly AppDbContext _context;

        public CatalogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Keys)
                .Select(g => new
                {
                    Id = g.id_игры,
                    Title = g.название,
                    Description = g.описание,
                    Price = g.цена,
                    ImagePath = g.изображение, 
                    Developer = g.Developer.разработчик,
                    Year = g.год_выпуска,
                    AvailableKeys = g.Keys.Count(k => !k.продан.HasValue || k.продан == false)
                })
                .Where(g => g.AvailableKeys > 0)
                .ToListAsync();

            return View(games);
        }
    }
}
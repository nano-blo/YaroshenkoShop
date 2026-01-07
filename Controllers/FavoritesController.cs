using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using YaroshenkoShop.Data;
using YaroshenkoShop.Models;

namespace YaroshenkoShop.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public FavoritesController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var favorites = await _context.Favorites
                .Where(f => f.UserId == user.Id)
                .Include(f => f.Game)
                .Select(f => f.Game)
                .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var exists = await _context.Favorites
                .AnyAsync(f => f.UserId == user.Id && f.GameId == id);

            if (!exists)
            {
                var favorite = new Favorite
                {
                    UserId = user.Id,
                    GameId = id
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Catalog");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == user.Id && f.GameId == id);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Favorites");
        }
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Importer.Domain.Entities;
using Importer.WApp.Data;
using Importer.WApp.Pages.Catalogs.Pokemons;

namespace Importer.WApp.Pages.Catalogs.Films
{
    public class IndexModel : PageModel
    {
        private readonly Importer.WApp.Data.PokemonDbContext _context;

        [BindProperty]
        public string SearchTerm { get; set; }

        public IndexModel(Importer.WApp.Data.PokemonDbContext context)
        {
            _context = context;
        }

        public IList<Film> Film { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is var userId && string.IsNullOrEmpty(userId))
                return Redirect("/index");

            if (_context.Films != null)
            {
                Film = await _context.Films.Where(x => x.UserId == Guid.Parse(userId)).ToListAsync();
                HttpContext.Session.Set("Films", Film);
            }

            return Page();
        }

        public async Task OnPostAsync(string searchTerm)
        {
            Film = HttpContext.Session.Get<IList<Film>>("Films");

            if (!string.IsNullOrEmpty(searchTerm))
                Film = Film.Where(x => x.Id.ToString().Contains(searchTerm) || x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
        }
    }
}

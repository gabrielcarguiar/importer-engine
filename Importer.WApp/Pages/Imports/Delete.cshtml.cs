using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Importer.Domain.Entities;
using Importer.WApp.Data;

namespace Importer.WApp.Pages.Imports.Films
{
    public class DeleteImportModel : PageModel
    {
        private readonly Importer.WApp.Data.PokemonDbContext _context;

        public DeleteImportModel(Importer.WApp.Data.PokemonDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Import Import { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Imports == null)
            {
                return NotFound();
            }

            var import = await _context.Imports.FirstOrDefaultAsync(m => m.Id == id);

            if (import == null)
            {
                return NotFound();
            }
            else 
            {
                Import = import;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Imports == null)
            {
                return NotFound();
            }
            var import = await _context.Imports.FindAsync(id);

            if (import != null)
            {
                Import = import;
                _context.Imports.Remove(Import);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

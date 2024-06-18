using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Radyuk.UI.Data;

using BAG.DOMAIN.Entities;

namespace Radyuk.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Bagi bagi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Bagi == null)
            {
                return NotFound();
            }

            var bagi= await _context.Bagi.FirstOrDefaultAsync(m => m.BagId == id);
            if (bagi == null)
            {
                return NotFound();
            }
            else
            {
                bagi = bagi;
            }
            return Page();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radyuk.UI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


using BAG.DOMAIN.Entities;

namespace Radyuk.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _environment;
        public EditModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }


        [BindProperty]
        public Bagi bagi { get; set; } = default!;
        [BindProperty]
        public IFormFile Image { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Bagi == null)
            {
                return NotFound();
            }

            var bagi = await _context.Bagi.FirstOrDefaultAsync(m => m.BagId == id);
            if (bagi == null)
            {
                return NotFound();
            }
            bagi = bagi;
            ViewData["GroupId"] = new SelectList(_context.Bagi, "GroupId", "GroupName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Image != null)
            {
                var fileName = $"{bagi.BagId}" +
                Path.GetExtension(Image.FileName);
                bagi.Image = fileName;
                var path = Path.Combine(_environment.WebRootPath, "Images",
                fileName);
                using (var fStream = new FileStream(path, FileMode.Create))
                {
                    await Image.CopyToAsync(fStream);
                }
            }


            _context.Attach(bagi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(bagi.BagId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DishExists(int id)
        {
            return _context.Bagi.Any(e => e.BagId == id);
        }
    }
}
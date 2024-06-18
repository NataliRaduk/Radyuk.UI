using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radyuk.UI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


using BAG.DOMAIN.Entities;
using Radyuk.UI.Services;

namespace Radyuk.UI.Areas.Admin.Pages
{
    public class CreateModel(ICategoryService categoryService, IProductService productService) : PageModel
    {
     
            public async Task<IActionResult> OnGet()
            {
                var categoryListData = await categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
                "GroupName");
                return Page();
            }
            [BindProperty]
            public Bagi bagi { get; set; } = default!;
            [BindProperty]
            public IFormFile? Image { get; set; }

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                await productService.CreateProductAsync(bagi, Image);
                return RedirectToPage("./Index");
            }
        }
    }

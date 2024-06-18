
using BAG.DOMAIN.Cart;
using Radyuk.UI.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Radyuk.UI.Extensions;

namespace Radyuk.UI.Components
{
	public class CartViewComponent: ViewComponent
	{
        
            public IViewComponentResult Invoke()
            {
                var cart = HttpContext.Session.Get<Cart>("cart");
                return View(cart);
            }
        }
    }


using BAG.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.DOMAIN.Cart
{
    public class CartItem
    {
        public Bagi Item { get; set; }
        public int Qty { get; set; }

    }
}

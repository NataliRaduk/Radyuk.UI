using BAG.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.DOMAIN.Cart
{
    public class Cart
    {
        public int Id { get; set; }
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="bagi">Добавляемый объект</param>
        public virtual void AddToCart(Bagi bagi)
        {
            if (CartItems.ContainsKey(bagi.BagId))
            {
                CartItems[bagi.BagId].Qty++;
            }
            else
            {
                CartItems.Add(bagi.BagId, new CartItem
                {
                    Item = bagi,
                    Qty = 1
                });
            };
        }
        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="bagi">удаляемый объект</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }
        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Qty); }
        /// <summary>
        /// Общее количество калорий
        /// </summary>
        
    }

}


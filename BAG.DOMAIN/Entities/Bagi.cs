using BAG.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.DOMAIN.Entities
{
	public class Bagi
	{
		[Key]
		public int BagId { get; set; } // id блюда
		public string BagName { get; set; } // название блюда
		public string Description { get; set; } // описание блюда

		public string? Image { get; set; } // имя файла изображения 

		// Навигационные свойства
		public int CategoryId { get; set; }
		public Category? Category { get; set; }

	}
}

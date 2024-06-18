
using BAG.DOMAIN.Entities;
using BAG.DOMAIN.Models;
using Microsoft.AspNetCore.Mvc;

namespace Radyuk.UI.Services
{
	public class MemoryProductService : IProductService
	{
		List<Bagi> _bagi;
		List<Category> _categories;
		IConfiguration _config;



		public MemoryProductService(ICategoryService categoryService, [FromServices] IConfiguration config)
		{
			_config = config;
			_categories = categoryService.GetCategoryListAsync()
				.Result
				.Data;

			SetupData();


		}



		/// <summary>
		/// Инициализация списков
		/// </summary>
		public void SetupData()
		{

			_bagi = new List<Bagi>
		{
			new Bagi {BagId = 1, BagName="Сумка женская 01",
			Description="Сумка кожа",
			Image="Images/01.png",
			CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Городской стиль")).Id},

			new Bagi {BagId = 1, BagName="Сумка мужская 02",
			Description="Сумка кожа",
			Image="Images/02.png",
			CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Городской стиль")).Id},

			new Bagi {BagId = 1, BagName="Сумка женская 03",
			Description="Сумка спортивная",
			Image="Images/03.png",
			CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Спортивный стиль")).Id},

            new Bagi {BagId = 1, BagName="Рюкзак женский 04",
			Description="Рюкзак экокожа",
			Image="Images/04.png",
			CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Спортивный стиль")).Id},

			new Bagi {BagId = 1, BagName="Сумка женская 05",
			Description="Сумка кожа",
			Image="Images/05.png",
			CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Городской стиль")).Id}


		};

		}
		public Task<ResponseData<ListModel<Bagi>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
		{


			// Создать объект результата
			var result = new ResponseData<ListModel<Bagi>>();

			// Id категории для фильрации
			int? categoryId = null;

			// если требуется фильтрация, то найти Id категории
			// с заданным categoryNormalizedName
			if (categoryNormalizedName != null)
				categoryId = _categories
				.Find(c =>
				c.NormalizedName.Equals(categoryNormalizedName))
				?.Id;

			// Выбрать объекты, отфильтрованные по Id категории,
			// если этот Id имеется
			var data = _bagi
			.Where(d => categoryNormalizedName == null || d.CategoryId.Equals(categoryNormalizedName))?
			.ToList();

			// получить размер страницы из конфигурации
			int pageSize = _config.GetSection("ItemsPerPage").Get<int>();


			// получить общее количество страниц
			int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

			// получить данные страницы
			var listData = new ListModel<Bagi>()
			{
				Items = data.Skip((pageNo - 1) *
			pageSize).Take(pageSize).ToList(),
				CurrentPage = pageNo,
				TotalPages = totalPages
			};

			// поместить ранные в объект результата
			result.Data = listData;



			// Если список пустой
			if (data.Count == 0)
			{
				result.Success = false;
				result.ErrorMessage = "Нет объектов в выбраннной категории";
			}
			// Вернуть результат
			return Task.FromResult(result);

		}

		public Task<ResponseData<Bagi>> GetProductByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateProductAsync(int id, Bagi product, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}

		public Task DeleteProductAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ResponseData<Bagi>> CreateProductAsync(Bagi product, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}

	}
}

using BAG.DOMAIN.Entities;
using BAG.DOMAIN.Models;

namespace Radyuk.UI.Services
{
	public class MemoryCategoryService: ICategoryService
	{
		public Task<ResponseData<List<Category>>> GetCategoryListAsync()
		{
			var categories = new List<Category>
			{
			new Category {Id=1, GroupName="Сумки",
			NormalizedName="Городской стиль"},
			new Category {Id=2, GroupName="Рюкзаки",
			NormalizedName="Спортивный стиль"}

			};
			var result = new ResponseData<List<Category>>();
			result.Data = categories;
			return Task.FromResult(result);
		}
	}
}

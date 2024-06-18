using BAG.DOMAIN.Entities;
using BAG.DOMAIN.Models;

namespace Radyuk.UI.Services
{
	public interface ICategoryService
	{


		/// <summary>
		/// Получение списка всех категорий
		/// </summary>
		/// <returns></returns>
		public Task<ResponseData<List<Category>>> GetCategoryListAsync();
	}
}

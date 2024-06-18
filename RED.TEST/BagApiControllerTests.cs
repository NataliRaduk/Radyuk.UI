using BAG.DOMAIN.Entities;
using BAG.DOMAIN.Models;
using Radyuk.API.Controllers;
using Radyuk.API.Data;
using Radyuk.UI.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace Radyuk.TESTS
{
	public class BagApiControllerTests : IDisposable
	{
		private readonly DbConnection _connection;
		private readonly DbContextOptions<AppDbContext> _contextOptions;
		private readonly IWebHostEnvironment _environment;
		public BagApiControllerTests()
		{
			_environment = Substitute.For<IWebHostEnvironment>();

			// Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
			// at the end of the test (see Dispose below).

			_connection = new SqliteConnection("Filename=:memory:");
			_connection.Open();

			// These options will be used by the context instances in this test suite, including the connection opened above.
			_contextOptions = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(_connection)
			.Options;

			// Create the schema and seed some data
			using var context = new AppDbContext(_contextOptions);

			context.Database.EnsureCreated();

			var categories = new Category[]
			{
			new Category {GroupName="", NormalizedName="Городской стиль"},
			new Category {GroupName="", NormalizedName="Спортивный стиль"},


			};
			context.Categories.AddRange(categories);
			context.SaveChanges();


			var bagis = new List<Bagi>
		{
		new Bagi {BagName="", Description="", Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("Городской стиль"))},

		new Bagi { BagName = "", Description = "", Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("Городской стиль")) },

		new Bagi { BagName = "", Description = "", Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("Спортивный стиль")) },

		new Bagi { BagName = "", Description = "", Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("Спортивный стиль")) },

		new Bagi { BagName = "", Description = "", Category = categories.FirstOrDefault(c => c.NormalizedName.Equals("Городской стиль")) }
		};
			context.AddRange(bagis);
			context.SaveChanges();
		}
		public void Dispose() => _connection?.Dispose();
		AppDbContext CreateContext() => new AppDbContext(_contextOptions);

		// Проверка фильтра по категории
		[Fact]
		public async void ControllerFiltersCategory()
		{
			// arrange
			using var context = CreateContext();

			var category = context.Categories.First();

			var controller = new BagiController(context, _environment);

			// act
			var response = await controller.GetProductListAsync(category.NormalizedName);
			ResponseData<ListModel<Bagi>> responseData = response.Value;
			var bagiList = responseData.Data.Items; // полученный список объектов
													  //assert
			Assert.True(bagiList.All(d => d.CategoryId == category.Id));
		}
		// Проверка подсчета количества страниц
		// Первый параметр - размер страницы
		// Второй параметр - ожидаемое количество страниц (при условии, что всего объектов 5)
		[Theory]
		[InlineData(2, 3)]
		[InlineData(3, 2)]
		public async void ControllerReturnsCorrectPagesCount(int size, int qty)
		{
			using var context = CreateContext();
			var controller = new BagiController(context, _environment);

			// act
			var response = await controller.GetProductListAsync(null, 1, size);
			ResponseData<ListModel<Bagi>> responseData = response.Value;
			var totalPages = responseData.Data.TotalPages; // полученное количество страниц
														   //assert
			Assert.Equal(qty, totalPages); // количество страниц совпадает
		}
		[Fact]
		public async void ControllerReturnsCorrectPage()
		{
			using var context = CreateContext();
			var controller = new BagiController(context, _environment);


			// При размере страницы 3 и общем количестве объектов 5
			// на 2-й странице должно быть 2 объекта
			int itemsInPage = 2;

			// Первый объект на второй странице
			Bagi firstItem = context.Bagi.ToArray()[3];

			// act
			// Получить данные 2-й страницы
			var response = await controller.GetProductListAsync(null, 2);
			ResponseData<ListModel<Bagi>> responseData = response.Value;
			var amgesList = responseData.Data.Items; // полученный список объектов
			var currentPage = responseData.Data.CurrentPage; // полученный номер текущей страницы
															 //assert
			Assert.Equal(2, currentPage);// номер страницы совпадает
			Assert.Equal(2, amgesList.Count); // количество объектов на странице равно 2
			Assert.Equal(firstItem.BagId, amgesList[0].BagId); // 1-й объект в списке правильный
		}
	}
}

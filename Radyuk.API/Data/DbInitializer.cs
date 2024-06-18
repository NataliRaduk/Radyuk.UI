using BAG.DOMAIN.Entities;
using BAG.DOMAIN.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;


namespace Radyuk.API.Data
{
    public static class DbInitializer
    {

        public static async Task SeedData(WebApplication app)
        {
            
            // Uri проекта
            var uri = "https://localhost:7002/";
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнение миграций
            await context.Database.MigrateAsync();

            if (!context.Categories.Any() && !context.Bagi.Any())
            {
                var _categories = new Category[]
                {
                    new Category {GroupName="Сумки", NormalizedName="Городской стиль"},

                    new Category {GroupName="Рюкзаки", NormalizedName="Спортивный стиль"}

                };
                await context.Categories.AddRangeAsync(_categories);
                await context.SaveChangesAsync();
 
                
               var _bagis = new List<Bagi>
        {
            new Bagi {BagName="Сумка женская 01",
            Description="Сумка кожа",
            Image= uri + "Images/01.png",
            Category = _categories.FirstOrDefault(c=>c.NormalizedName.Equals("Городской стиль"))},

            new Bagi {BagName="Сумка женская 02",
            Description="Сумка кожа",
            Image= uri + "Images/02.png",
            Category = _categories.FirstOrDefault(c=>c.NormalizedName.Equals("Городской стиль"))},

            new Bagi {BagName="Сумка женская 03",
            Description="Сумка спортивная",
            Image= uri + "Images/03.png",
            Category = _categories.FirstOrDefault(c=>c.NormalizedName.Equals("Спортивный стиль"))},

            new Bagi {BagName="Сумка женская 04",
            Description="Рюкзак экокожа",
            Image= uri + "Images/04.png",
            Category = _categories.FirstOrDefault(c=>c.NormalizedName.Equals("Спортивный стиль"))},

            new Bagi {BagName="Сумка женская 05",
            Description="Сумка кожа",
            Image= uri + "Images/05.png",
            Category = _categories.FirstOrDefault(c=>c.NormalizedName.Equals("Городской стиль"))},




        };
                await context.Bagi.AddRangeAsync(_bagis);
                await context.SaveChangesAsync();

            }
        }
    }
}

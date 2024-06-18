using BAG.DOMAIN.Entities;
using BAG.DOMAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radyuk.API.Data;

namespace Radyuk.API.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class BagiController : ControllerBase
        {
            private readonly AppDbContext _context;


            public BagiController(AppDbContext context, IWebHostEnvironment _environment)
            {
                _context = context;

            }

            

            [HttpGet]
            public async Task<ActionResult<ResponseData<ListModel<Bagi>>>> GetProductListAsync
                (string? category,
                int pageNo = 1, int pageSize = 6)
            {
                // Создать объект результата
                var result = new ResponseData<ListModel<Bagi>>();




                //// Фильтрация по категории загрузка данных категории
                var data = _context.Bagi
                .Include(d => d.Category)
                .Where(d => String.IsNullOrEmpty(category)
                || d.Category.NormalizedName.Equals(category));



                // Подсчет общего количества страниц
                int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);
                if (pageNo > totalPages)
                    pageNo = totalPages;

                // Создание объекта ProductListModel с нужной страницей данных
                var listData = new ListModel<Bagi>()
                {
                    Items = await data
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages
                };

                // поместить данные в объект результата
                result.Data = listData;

                // Если список пустой
                if (data.Count() == 0)
                {
                    result.Success = false;
                    result.ErrorMessage = "Нет объектов в выбранной категории";
                }
                return result;
            }


            
            [HttpGet("{id}")]
            public async Task<ActionResult<Bagi>> GetBagi(int id)
            {
                var bagi = await _context.Bagi.FindAsync(id);

                if (bagi == null)
                {
                    return NotFound();
                }

                return bagi;
            }

            
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPut("{id}")]
            public async Task<IActionResult> PutBagi(int id, Bagi bagi)
            {
                if (id != bagi.BagId)
                {
                    return BadRequest();
                }

                _context.Entry(bagi).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BagiExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

       
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public async Task<ActionResult<Bagi>> PostBagi(Bagi bagi)
            {
                _context.Bagi.Add(bagi);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBagi", new { id = bagi.BagId }, bagi);
            }

            
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteBagi(int id)
            {
                var bagi = await _context.Bagi.FindAsync(id);
                if (bagi == null)
                {
                    return NotFound();
                }

                _context.Bagi.Remove(bagi);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool BagiExists(int id)
            {
                return _context.Bagi.Any(e => e.BagId == id);
            }

            [HttpPost("{id}")]

            public async Task<IActionResult> SaveImage(int id, IFormFile image, [FromServices] IWebHostEnvironment env)
            {
                // Найти объект по Id
                var bagi = await _context.Bagi.FindAsync(id);
                if (bagi == null)
                {
                    return NotFound();
                }

                // Путь к папке wwwroot/Images
                var imagesPath = Path.Combine(env.WebRootPath, "Images");

                // получить случайное имя файла
                var randomName = Path.GetRandomFileName();

                // получить расширение в исходном файле
                var extension = Path.GetExtension(image.FileName);

                // задать в новом имени расширение как в исходном файле
                var fileName = Path.ChangeExtension(randomName, extension);

                // полный путь к файлу
                var filePath = Path.Combine(imagesPath, fileName);

                // создать файл и открыть поток для записи
                using var stream = System.IO.File.OpenWrite(filePath);

                // скопировать файл в поток
                await image.CopyToAsync(stream);

                // получить Url хоста
                var host = "https://" + Request.Host;

                // Url файла изображения
                var url = $"{host}/Images/{fileName}";

                // Сохранить url файла в объекте
                bagi.Image = url;
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }


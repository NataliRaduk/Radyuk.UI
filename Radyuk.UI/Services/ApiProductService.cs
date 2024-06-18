using BAG.DOMAIN.Entities;
using BAG.DOMAIN.Models;
using System.Text.Json;

namespace Radyuk.UI.Services
{
    
        public class ApiProductService(HttpClient httpClient) : IProductService
        {
            public async Task<ResponseData<Bagi>> CreateProductAsync(Bagi product, IFormFile? formFile)
            {
                var serializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Подготовить объект, возвращаемый методом
                var responseData = new ResponseData<Bagi>();

                // Послать запрос к API для сохранения объекта
                var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, product);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось создать объект:{response.StatusCode}";
                    return responseData;
                }

                // Если файл изображения передан клиентом
                if (formFile != null)
                {

                    // получить созданный объект из ответа Api-сервиса
                    var bagi = await response.Content.ReadFromJsonAsync<Bagi>();

                    // создать объект запроса
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}{bagi.BagId}")
                    };

                    // Создать контент типа multipart form-data
                    var content = new MultipartFormDataContent();

                    // создать потоковый контент из переданного файла
                    var streamContent = new StreamContent(formFile.OpenReadStream());

                    // добавить потоковый контент в общий контент по именем "image"
                    content.Add(streamContent, "image", formFile.FileName);

                    // поместить контент в запрос
                    request.Content = content;

                    // послать запрос к Api-сервису
                    response = await httpClient.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        responseData.Success = false;
                        responseData.ErrorMessage = $"Не удалось сохранить изображение:{response.StatusCode} ";
                    }
                }
                return responseData;
            }


            public Task DeleteProductAsync(int id)
            {
                throw new NotImplementedException();
            }

            public async Task<ResponseData<Bagi>> GetProductByIdAsync(int id)
            {
                var apiUrl = $"{httpClient.BaseAddress.AbsoluteUri}{id}";
                var response = await httpClient.GetFromJsonAsync<Bagi>(apiUrl);
                return new ResponseData<Bagi>() { Data = response };
            }

            public async Task<ResponseData<ListModel<Bagi>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
            {
                var uri = httpClient.BaseAddress;
                var queryData = new Dictionary<string, string>();
                queryData.Add("pageNo", pageNo.ToString());
                if (!String.IsNullOrEmpty(categoryNormalizedName))
                {
                    queryData.Add("category", categoryNormalizedName);
                }
                var query = QueryString.Create(queryData);
                var result = await httpClient.GetAsync(uri + query.Value);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content
                    .ReadFromJsonAsync<ResponseData<ListModel<Bagi>>>();
                };
                var response = new ResponseData<ListModel<Bagi>>
                { Success = false, ErrorMessage = "Ошибка чтения API" };
                return response;
            }

            public Task UpdateProductAsync(int id, Bagi product, IFormFile? formFile)
            {
                throw new NotImplementedException();
            }
        }
    }


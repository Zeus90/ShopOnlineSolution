using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var respone = await this.httpClient.GetAsync($"/api/Product");

                if (respone.IsSuccessStatusCode)
                {
                    if (respone.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    return await respone.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    var message = await respone.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            try
            {
                var respone = await this.httpClient.GetAsync($"/api/Product/{id}");

                if (respone.IsSuccessStatusCode)
                {
                    if (respone.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }

                    return await respone.Content.ReadFromJsonAsync<ProductDto>();
                }
                else
                {
                    var message = await respone.Content.ReadAsStringAsync();
                    throw new Exception(message);   
                }
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }
    }
}

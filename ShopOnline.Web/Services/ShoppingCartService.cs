using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;

        public ShoppingCartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto itemToAdd)
        {
            try
            {
                var res = await this.httpClient.PostAsJsonAsync<CartItemToAddDto>("/api/ShoppingCart", itemToAdd);

                if (res.IsSuccessStatusCode)
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }

                    return await res.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await res.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<CartItemDto> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CartItemDto>> GetAllItems(int userId)
        {
            try
            {
                var res = await this.httpClient.GetAsync($"/api/ShoppingCart/{userId}/GetCartItems");

                if (res.IsSuccessStatusCode)
                {
                    if (res.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>();
                    }

                    return await res.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();
                }
                else
                {
                    var message = await res.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<CartItemDto> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemDto> UpdateItem(int id, CartItemQtyUpdateDto itemQtyToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}

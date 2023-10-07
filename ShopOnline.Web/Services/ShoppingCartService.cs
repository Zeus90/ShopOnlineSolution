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

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var res = await this.httpClient.DeleteAsync($"/api/ShoppingCart/{id}");

                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadFromJsonAsync<CartItemDto>();
                }

                return default(CartItemDto);
            }
            catch (Exception)
            {
                //log exception
                throw;
            }
        }

        public async Task<List<CartItemDto>> GetAllItems(int userId)
        {
            try
            {
                var res = await this.httpClient.GetAsync($"/api/ShoppingCart/{userId}/GetCartItems");

                if (res.IsSuccessStatusCode)
                {
                    if (res.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }

                    return await res.Content.ReadFromJsonAsync<List<CartItemDto>>();
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

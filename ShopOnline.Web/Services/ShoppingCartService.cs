using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;
        public event Action<int> OnShoppingCartChanged;
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

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            //check if event has subsicribers
            if (OnShoppingCartChanged != null)
            {
                //raise the even for the subs
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }

        public async Task<CartItemDto> UpdateItem(CartItemQtyUpdateDto itemQtyToUpdate)
        {
            try
            {
                //serialize the object
                var jsonRequest = JsonConvert.SerializeObject(itemQtyToUpdate);
                //pass data as stringContentObject formate to server
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var res = await httpClient.PatchAsync($"/api/ShoppingCart/{itemQtyToUpdate.CartItemId}", content);

                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return null;
            }
            catch (Exception)
        {
                //log exception
                throw;
            }
        }
    }
}

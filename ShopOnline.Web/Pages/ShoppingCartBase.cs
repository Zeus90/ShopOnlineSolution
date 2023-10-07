using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShopingCartItemsDto { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShopingCartItemsDto = await ShoppingCartService.GetAllItems(HardCoded.UserId);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task Delete_Click(int id)
        {
            var item = await this.ShoppingCartService.DeleteItem(id);

            RemoveItem(item.Id);
        }

        private CartItemDto GetItemDto(int id)
        {
            return ShopingCartItemsDto.FirstOrDefault(x => x.Id == id);
        }

        private async Task RemoveItem(int id)
        {
            var cartItemDto = GetItemDto(id);
            ShopingCartItemsDto.Remove(cartItemDto);
        }
    }
}

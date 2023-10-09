using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShopingCartItemsDto { get; set; }
        public string ErrorMessage { get; set; }

        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShopingCartItemsDto = await ShoppingCartService.GetAllItems(HardCoded.UserId);
                CartChanged();
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

            CartChanged();
        }

        protected async Task UpdateQty_Click(int cartItemId, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = cartItemId,
                        Qty = qty
                    };

                    var returnedUpdatedItemQty = await ShoppingCartService.UpdateItem(updateItemDto);

                    UpdateItemTotalPrice(returnedUpdatedItemQty);

                    CartChanged();

                    await MakeUpdateBtnStyle(cartItemId, false);
                }
                else
                {
                    var item = await ShoppingCartService.GetItem(cartItemId);

                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task UpdateQtyButton_Input(int id)
        {
            await MakeUpdateBtnStyle(id, true);
        }


        private async Task MakeUpdateBtnStyle(int id, bool visible)
        {
            await Js.InvokeVoidAsync("UpdateQtyBtn", id, visible);
        }

        private void UpdateItemTotalPrice(CartItemDto returnedUpdatedItemQty)
        {
            var item = GetItemDto(returnedUpdatedItemQty.Id);

            if (item != null)
            {
                item.TotalPrice = item.Price * item.Qty;
            }
        }
        
        private void CalculateCartSummery()
        {
            SetTotalPice();
            SetTotalQuantity();
        }

        private void SetTotalPice()
        {
            TotalPrice = ShopingCartItemsDto.Sum(p => p.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShopingCartItemsDto.Sum(p => p.Qty);
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

        private void CartChanged()
        {
            CalculateCartSummery();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }
    }
}

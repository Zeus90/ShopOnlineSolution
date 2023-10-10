using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class CheckoutBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }
        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
        protected string PaymentDescription { get; set; }
        protected decimal PaymentAmount { get; set; }
        protected int TotalQty { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await CartItemsLocalStorageService.GetCollection();

                if (ShoppingCartItems != null)
                {
                    Guid paymentGuid = Guid.NewGuid();

                    PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                    PaymentDescription = $"O_{HardCoded.UserId}_{paymentGuid}";
                }
            }
            catch (Exception)
            {

                throw;
            }

            //if event occured after first render
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                //use argument to ensure that the code in this method fired only once after component is renderd
                if (firstRender)
                {
                    //call js method
                    await Js.InvokeVoidAsync("initPayPalButton");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

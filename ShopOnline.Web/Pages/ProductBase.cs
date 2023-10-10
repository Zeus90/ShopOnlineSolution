using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppinCartService { get; set; }
        public ProductDto Product { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }
        [Parameter]
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        private List<CartItemDto> ShoppingCartItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await CartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddItem_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await ShoppinCartService.AddItem(cartItemToAddDto);
                if (cartItemDto != null)
                {
                    ShoppingCartItems.Add(cartItemDto);
                    await CartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<ProductDto> GetProductById(int id)
        {
            var productsDto = await ProductsLocalStorageService.GetCollection();

            if (productsDto != null)
            {
                return productsDto.SingleOrDefault(p => p.Id == id);
            }

            return null;
        }
    }
}

using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ClearLocalStorage();

            Products = await ProductsLocalStorageService.GetCollection();

            var shoppingCartItems = await CartItemsLocalStorageService.GetCollection();

            var totalQty = shoppingCartItems.Sum(p => p.Qty);

            ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupProductDto)
        {
            return groupProductDto.FirstOrDefault(gp => gp.CategoryId == groupProductDto.Key).CategoryName;
        }

        private async Task ClearLocalStorage()
        {
            await ProductsLocalStorageService.RemoveCollection();
            await CartItemsLocalStorageService.RemoveCollection();
        }
    }
}

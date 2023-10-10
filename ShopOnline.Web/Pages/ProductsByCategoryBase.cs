using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsByCategoryBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductDto> ProductsByCategory { get; set; }
        [Parameter]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                ProductsByCategory = await ProductService.GetProductsByCategoryId(CategoryId);

                if (ProductsByCategory != null && ProductsByCategory.Count() > 0)
                {
                    var product = ProductsByCategory.FirstOrDefault(p => p.CategoryId == CategoryId);

                    if (product != null)
                    {
                        CategoryName = product.CategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
    }
}

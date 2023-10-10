using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetItems();
        Task<ProductDto> GetProductById(int id);
        Task<IEnumerable<ProductCategoryDto>> GetProductCategories();
        Task <IEnumerable<ProductDto>> GetProductsByCategoryId(int categoryId);
    }
}

using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartItemDto> AddItem(CartItemToAddDto itemToAdd);
        Task<CartItemDto> UpdateItem(int id, CartItemQtyUpdateDto itemQtyToUpdate);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> GetItem(int id);
        Task<IEnumerable<CartItemDto>> GetAllItems(int userId);
    }
}

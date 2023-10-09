using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartItemDto> AddItem(CartItemToAddDto itemToAdd);
        Task<CartItemDto> UpdateItem(CartItemQtyUpdateDto itemQtyToUpdate);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> GetItem(int id);
        Task<List<CartItemDto>> GetAllItems(int userId);
        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQty);
    }
}

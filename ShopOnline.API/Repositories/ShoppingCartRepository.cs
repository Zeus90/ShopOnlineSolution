using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineContext _context;

        public ShoppingCartRepository(ShopOnlineContext context)
        {
            this._context = context;
        }
        private async Task<bool> CartItemExist(int cartId, int productId)
        {
            return await this._context.CartItems.AnyAsync(c => c.CartId == cartId &&
            c.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExist(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in this._context.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = product.Id,
                                      Qty = cartItemToAddDto.Qty
                                  }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var res =  await this._context.CartItems.AddAsync(item);
                    await this._context.SaveChangesAsync();
                    return res.Entity;
                }
            }

            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await this._context.CartItems.FindAsync(id);

            if (item != null)
        {
                this._context.CartItems.Remove(item);
                    await this._context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<IEnumerable<CartItem>> GetAllItems(int userId)
        {
            return await (from cart in this._context.Carts
                          join cartItem in this._context.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).ToListAsync();
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in this._context.Carts
                          join cartItem in this._context.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await this._context.CartItems.FindAsync(id);
            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await this._context.SaveChangesAsync();

                return item;
            }
            return null;
        }
    }

}

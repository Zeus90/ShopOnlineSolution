using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Extentions
{
    public static class DtosConversion
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products,
                                                            IEnumerable<ProductCategory> categories)
        {
            return (from product in products
                   join productCategory in categories
                   on product.CategoryId equals productCategory.Id
                   select new ProductDto
                   {
                       Id = product.Id,
                       Name = product.Name,
                       Description = product.Description,
                       ImageURL = product.ImageURL,
                       Price = product.Price,
                       Qty = product.Qty,
                       CategoryId = product.CategoryId,
                       CategoryName = productCategory.Name
                   }).ToList();
        }

        public static ProductDto ConvertToDto(this Product product, ProductCategory category)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.CategoryId,
                CategoryName = category.Name
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> items, IEnumerable<Product> products)
        {
            return (from product in products
                   join item in items
                   on product.Id equals item.ProductId
                   select new CartItemDto
                   {
                       Id = item.Id,
                       ProductId = product.Id,
                       Qty = item.Qty,
                       CartId = item.CartId,
                       ProductName = product.Name,
                       ProductDescription = product.Description,
                       Price= product.Price,
                       ProductImageURL = product.ImageURL,
                       TotalPrice = product.Price * item.Qty
                   }).ToList();
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = product.Id,
                CartId = cartItem.CartId,
                Qty = cartItem.Qty,
                ProductName = product.Name,
                ProductDescription = product.Description,
                Price = product.Price,
                ProductImageURL = product.ImageURL,
                TotalPrice = product.Price * cartItem.Qty
            };
        }
    }
}

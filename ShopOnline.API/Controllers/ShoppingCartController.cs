using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extentions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;


        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }


        [HttpGet]
        [Route("{userId}/GetCartItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems(int userId)
        {
            try
            {
                var cartItems = await this.shoppingCartRepository.GetAllItems(userId);

                if (cartItems == null)
                {
                    return NoContent();
                }

                var products = await this.productRepository.GetItems();

                if (products == null)
                {
                    throw new Exception("No products exist in the system");
                }

                var cartItemsDto = cartItems.ConvertToDto(products);
                return Ok(cartItemsDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
            try
            {
                var cartItem = await this.shoppingCartRepository.GetItem(id);

                if (cartItem == null)
                {
                    return NotFound();
                }

                var product = await this.productRepository.GetItem(cartItem.ProductId);

                if (product == null)
                {
                    return NoContent();
                }

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]

        public async Task<ActionResult<CartItemDto>> AddItem([FromBody] CartItemToAddDto cartItemToAdd)
        {
            try
            {
                var newCartItem = await this.shoppingCartRepository.AddItem(cartItemToAdd);
                if (newCartItem == null)
                {
                    return NoContent();
                }

                var product = await this.productRepository.GetItem(newCartItem.ProductId);
                if (product == null)
                {
                    throw new Exception($"Something went wrong while retrieving product: {cartItemToAdd.ProductId}");
                }

                var newCartItemDto = newCartItem.ConvertToDto(product);

                //the convention is to return the location of the newly post data =>

                return CreatedAtAction(nameof(GetItem), new { id = newCartItemDto.Id }, newCartItemDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                var item = await this.shoppingCartRepository.DeleteItem(id);

                if (item == null)
                {
                    return NotFound("Item not found controller layer");
                }

                var product = await this.productRepository.GetItem(item.ProductId);
                if (product == null)
                {
                    return NotFound("Product not found controller layer");
                }

                var itemDto = item.ConvertToDto(product);

                return Ok(itemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemQtyUpdateDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await this.shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);
                if (cartItem == null)
                {
                    return NotFound();
                }
                var product = await productRepository.GetItem(cartItem.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                var updatedCartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemQtyUpdateDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

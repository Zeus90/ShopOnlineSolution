using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extentions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                //two different queries to db is not ideal//use include!!
                var products = await productRepository.GetItems();
                var categories = await productRepository.GetCategories();

                if (products == null || categories == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDto = products.ConvertToDto(categories);

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Error retrieving data from database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetSingleItem(int id)
        {
            try
            {
                var product = await productRepository.GetItem(id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    var category = await productRepository.GetCategory(product.CategoryId);

                    var productDto = product.ConvertToDto(category);

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Ok();
        }
    }
}

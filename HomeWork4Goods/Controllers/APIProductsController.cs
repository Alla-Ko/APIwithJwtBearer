using HomeWork4Products.Models;
using HomeWork4Products.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork4Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIProductsController : ControllerBase
    {
        private readonly IServiceProducts _serviceProducts;

        public APIProductsController(IServiceProducts serviceProducts)
        {
            _serviceProducts = serviceProducts;

        }

        // GET: api/APIProducts
        //[Authorize]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string searchString = "")
        {


            var products = await _serviceProducts.ReadAsync(searchString);
            return Ok(products);

        }

        // GET: api/APIProducts/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int? id)
        {
            if (id == null)
            {
                return NotFound($"Product with id {id} is not found");
            }

            var product = await _serviceProducts.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);

        }
        [Authorize(Roles = "admin")]


        // POST: api/APIProducts
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "admin")]
        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return BadRequest(new { errors });
            }

            await _serviceProducts.CreateAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }



        // PUT: api/APIProducts/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var updatedProduct = await _serviceProducts.UpdateAsync(id, product);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/APIProducts/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _serviceProducts.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}

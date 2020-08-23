using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogRepository _repository;
        private const string ErrorMessage = "An error occurred while processing your request";

        public CatalogController(ICatalogRepository repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(List<Product>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Catalog()
        {
            var (IsSuccess, Products, errorType) = await _repository.GetProductsAsync() ;
            if (IsSuccess)
            {
                return Ok(Products.ToList());
            }
            if(errorType==ErrorType.NotFound)
                return NotFound();

            return BadRequest(ErrorMessage);
        }
        
        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Product>> Catalog(string id)
        {
            var result = await _repository.GetProductByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }
            if (result.errorType==ErrorType.NotFound)
            {
                return NotFound("Product not found");
            }
            
            return BadRequest(ErrorMessage);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("[action]/{category}")]
        public async Task<ActionResult> GetProductByCategory(string category)
        {
            var result = await _repository.GetProductsByCategoryIdAsync(category);
            if (result.IsSuccess)
            {
                return Ok(result.Products.ToList());
            }
            if(result.errorType==ErrorType.NotFound)
                return NotFound();

            return BadRequest(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Catalog([FromBody] Product product)
        {
            var (IsSuccess, errorType) = await _repository.AddProductAsync(product);
            if (IsSuccess)
            {
                return CreatedAtRoute("GetProduct",new { id=product.Id},product);
            }
            if (errorType==ErrorType.NotFound)
            {
                return NotFound();
            }
            return BadRequest(ErrorMessage);
        }

        [HttpPut]
        
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateCatalog(Product product)
        {
            var (IsSuccess, errorType) = await _repository.UpdateProductAsync(product);
            if (IsSuccess)
            {

                return Ok();
            }
            if(errorType==ErrorType.NotFound)
                return NotFound();

            return BadRequest(ErrorMessage);
        }

        [HttpDelete]
        
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteCatalog(string id)
        {
            var (IsSuccess, errorType) = await _repository.DeleteProductAsync(id);
            if (IsSuccess)
            {
                return Ok();
            }
            if(errorType==ErrorType.NotFound)
                return NotFound();

            return BadRequest(ErrorMessage);
        }
        
    }
}

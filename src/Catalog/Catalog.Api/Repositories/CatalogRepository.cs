using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly ICatalogContext _context;
        private const string ErrorMessage = "An error occurred while processing your request";
        private const string BadRequest = "Bad Request";
        public CatalogRepository(ICatalogContext context)
        {
            _context = context;
        }


        public async Task<(bool IsSuccess, ErrorType errorType)> AddProductAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    return (false,ErrorType.NotFound);
                }
                await _context.Products.InsertOneAsync(product);
                return (true, default);
            }
            catch (System.Exception ex)
            {
                //TODO: ADD Logging to seq
                return (false, ErrorType.InternalServerError);
            }


        }

        public async Task<(bool IsSuccess, ErrorType errorType)> DeleteProductAsync(string productId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productId))
                {
                    return (false, ErrorType.BadRequest);
                }
                await _context.Products.DeleteOneAsync(c => c.Id == productId);
                return (true, default);
            }
            catch (System.Exception ex)
            {

                return (false, ErrorType.InternalServerError);
            }

        }

        public async Task<(bool IsSuccess, Product Product, ErrorType errorType)> GetProductByIdAsync(string id)
        {
            try
            {
                var product = await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (product == null)
                {
                    return (false, null, ErrorType.NotFound);
                }
                return (true, product, default);

            }
            catch (System.Exception ex)
            {
                return (false, null, ErrorType.InternalServerError);
            };

        }

        public async Task<(bool IsSuccess, IEnumerable<Product> Products, ErrorType errorType)> GetProductsAsync()
        {
            List<Product> allProducts;

            try
            {
                allProducts = await _context.Products.Find(p => true).ToListAsync();
                if (allProducts == null)
                {
                    return (false, null, ErrorType.NotFound);
                }
                return (true, allProducts, default);
            }
            catch (System.Exception ex)
            {

                return (false, null, ErrorType.InternalServerError);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Product> Products, ErrorType errorType)> GetProductsByCategoryIdAsync(string categoryId)
        {
             if (string.IsNullOrWhiteSpace(categoryId))
            {
                return (false, null, ErrorType.BadRequest);
            }
            try
            {
                FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Category, categoryId);
                var prods = await _context.Products.Find(filter).ToListAsync();
                return (true, prods, default);
            }
            catch (System.Exception ex)
            {
                return (false, null, ErrorType.InternalServerError);
                throw;
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Product> Products, ErrorType errorType)> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (false, null,ErrorType.BadRequest);
            }
            try
            {
                FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);
                var prods = await _context.Products.Find(filter).ToListAsync();
                return (true, prods, default);
            }
            catch (System.Exception ex)
            {
                return (false, null, ErrorType.InternalServerError);
                
            }
        }

        public async Task<(bool IsSuccess, ErrorType errorType)> UpdateProductAsync(Product product)
        {
            try
            {
                var updateresult = await _context
                                                .Products
                                                .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

                return (updateresult.IsAcknowledged, updateresult.IsAcknowledged ? default : ErrorType.NotFound);
            }
            catch (System.Exception ex)
            {

                return (false, ErrorType.InternalServerError);
            }
        }
    }
}

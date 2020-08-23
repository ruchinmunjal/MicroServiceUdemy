using Catalog.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
    public interface ICatalogRepository
    {
        Task<(bool IsSuccess,IEnumerable<Product> Products,ErrorType errorType)> GetProductsAsync();
        Task<(bool IsSuccess,Product Product,ErrorType errorType)> GetProductByIdAsync(string id);

        Task<(bool IsSuccess,IEnumerable<Product> Products,ErrorType errorType)> GetProductsByCategoryIdAsync(string categoryId);

        Task<(bool IsSuccess,IEnumerable<Product> Products,ErrorType errorType)> GetProductsByNameAsync(string name);

        Task<(bool IsSuccess,ErrorType errorType)> AddProductAsync(Product product);

        Task<(bool IsSuccess,ErrorType errorType)> UpdateProductAsync(Product product);

        Task<(bool IsSuccess,ErrorType errorType)> DeleteProductAsync(string productId);


    }

    public enum ErrorType
    {
        BadRequest,
        NotFound,
        InternalServerError
    }
}

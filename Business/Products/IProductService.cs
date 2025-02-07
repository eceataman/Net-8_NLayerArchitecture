using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    public interface IProductService
    {
        Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count);
        Task<ServiceResult<ProductDto?>> GetByIdAsync(int id);
        Task<ServiceResult<List<ProductDto>>> GetAllListAsync();
        Task<ServiceResult<CreateProductsResponse>> CreateAsync(CreateProductsRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateProductsRequest request);
        Task<ServiceResult> DeleteAsync(int id);
    }
}

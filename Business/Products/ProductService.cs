
using App.Repositories;
using App.Repositories.Products;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)

        {

            var products = await productRepository.GetTopPriceProductAsync(count);

            var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productAsDto,
            };
        }
        public async Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                ServiceResult<Product>.Fail("Product not found", HttpStatusCode.NotFound);
            }
            var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);
            return ServiceResult<ProductDto>.Success(productAsDto!);
        }
        public async Task<ServiceResult<CreateProductsResponse>> CreateProductAsync(CreateProductsRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Stock = request.Stock,
                Price = request.Price,

            };
            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<CreateProductsResponse>.Success(new CreateProductsResponse(product.Id));
        }
        public async Task<ServiceResult> UpdateProductAsync(int id, UpdateProductsRequest request)
        {
            var product = await productRepository.GetByIdAsync(id);

            // Fast-fail: Önce başarısız durumları döndür
            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            // Ürün güncelleme
            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success();
        }
        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }
            productRepository.Delete(product);
            unitOfWork.SaveChangesAsync();
            return ServiceResult.Success();
            //Transaction servis katmanından yönetilir, repository katmanından değil

        }
    }
}
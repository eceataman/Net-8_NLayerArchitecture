
using App.Repositories;
using App.Repositories.Products;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var products = await productRepository.GetAll().ToListAsync();
            var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            return ServiceResult<List<ProductDto>>.Success(productAsDto);

        }
        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            // 1. sayfa 0-10 skip(0).take(10)  2. sayfa 11-20 skip 10 take 10 3.sayfa 21-30 skip 20 take 10 
            var products = await productRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
            return ServiceResult<List<ProductDto>>.Success(productAsDto);

        }
        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                ServiceResult<Product>.Fail("Product not found", HttpStatusCode.NotFound);
            }
            var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);
            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }
        public async Task<ServiceResult<CreateProductsResponse>> CreateAsync(CreateProductsRequest request)
        {
           
                var product = new Product()
                {
                    Name = request.Name,
                    Stock = request.Stock,
                    Price = request.Price,
                };

                await productRepository.AddAsync(product);
                await unitOfWork.SaveChangesAsync();

                Console.WriteLine($"Ürün başarıyla eklendi. ID: {product.Id}");
            return ServiceResult<CreateProductsResponse>.SuccessAsCreated(new CreateProductsResponse(product.Id), $"api/products/{product.Id}");


        }


        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductsRequest request)
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

            return ServiceResult.Success(HttpStatusCode.NoContent);
            //204 No Content, istemciye veri döndürülmesi gerekmediğinde veya döndürülecek bir içerik olmadığında kullanılır.
        }
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }
            productRepository.Delete(product);
            unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
            //Transaction servis katmanından yönetilir, repository katmanından değil

        }
    }
}
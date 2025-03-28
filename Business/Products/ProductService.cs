﻿
using App.Repositories;
using App.Repositories.Products;
using App.Services.ExceptionHandlers;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Builder;
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
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork,IMapper mapper) : IProductService
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
            #region manuel mapping 
            // var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
            #endregion
            // mapping
            var productAsDto = mapper.Map<List<ProductDto>>(products);


            return ServiceResult<List<ProductDto>>.Success(productAsDto);

        }
        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            // 1. sayfa 0-10 skip(0).take(10)  2. sayfa 11-20 skip 10 take 10 3.sayfa 21-30 skip 20 take 10 
            var products = await productRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            // var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
            var productAsDto = mapper.Map<List<ProductDto>>(products);
            return ServiceResult<List<ProductDto>>.Success(productAsDto);

        }
        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
               return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
            }
            // var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);
            var productAsDto = mapper.Map<ProductDto>(product);
            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }
        public async Task<ServiceResult<CreateProductsResponse>> CreateAsync(CreateProductsRequest request)
        {
            throw new CriticalException("kritik bir seviye hata meydana geldi");
            var anyProduct = await productRepository.Where(x => x.Name == request.Name).AnyAsync();
            if (anyProduct)
            {
                //bu asenkron programlama
                return ServiceResult<CreateProductsResponse>.Fail("Ürün veritabanında bulunmaktadır", HttpStatusCode.BadRequest);
            }
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
            var isProductNameExist=await productRepository.Where(x => x.Name==request.Name && x.Id != product.Id) .AnyAsync();
            //Bu kod, veritabanında aynı isimde ama farklı ID'ye sahip bir ürünün olup olmadığını kontrol ediyor. Yani, güncelleme işlemi yaparken, mevcut ürün dışında aynı isimde başka bir ürün var mı diye bakıyor.
            if (isProductNameExist)
            {
                return ServiceResult.Fail("ürün ismi veritabanında bulunmaktadır",
                    HttpStatusCode.BadRequest);
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
        public async Task<ServiceResult> UpdateProductStockAsync(UpdateProductStockRequest request)
        {
            var product=await productRepository.GetByIdAsync(request.productId);
            if(product is null)
            {
                return ServiceResult.Fail("Product doesnt found", HttpStatusCode.NotFound);
            }
            //clean codeda bir method 2den fazla değişken içeriyorsa bunu objeyw çevirmeliyiz. bu yüzden de updatestockrequest e çevirdim
            product.Stock = request.quantity;
            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
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
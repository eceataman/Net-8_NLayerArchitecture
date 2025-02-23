using App.Services;
using App.Services.Products;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.API.Controllers
{

    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            return CreateActionResult(await productService.GetAllListAsync());

        }
        // route constraint türünün int oldgunu belirtiyoruz.Route Constraint, ASP.NET Core'da URL parametrelerinin belirli kurallara uymasını sağlayan bir mekanizmadır.
       // Bu kısıtlamalar, doğru türde ve formatta veri geldiğinden emin olmak için kullanılır./products/5 → Geçerli (Çünkü 5 bir integer'dır.)
      // /products/apple → Geçersiz(Çünkü apple bir string'dir ve sayı bekleniyor.)


       [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber,int pageSize)
        {

            return CreateActionResult(await productService.GetPagedAllListAsync(pageNumber,pageSize));

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var serviceResult = await productService.GetByIdAsync(id);
            //return CreateActionResult();
            return CreateActionResult(await productService.GetByIdAsync(id));

            //amaç başarılı başarsız için ayrı ayrı döndürmeye gerek yok
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductsRequest request)
        {
            return CreateActionResult(await productService.CreateAsync(request));
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,UpdateProductsRequest request)
        {
            return CreateActionResult(await productService.UpdateAsync(id, request));
        }
        //PATCH, bir kaynağın tamamını değiştirmek yerine, yalnızca belirli alanlarını güncellemek için kullanılır.

        //PATCH = KISMİ GÜNCELLEME
        //     PUT = TAM GÜNCELLEME. Aynısını stock update i için putta da yapabiliriz sadece daha detaylı bir şekilde isim verilmeli. Updatestock vb. gibi
        [HttpPatch("stock")]
        public async Task<IActionResult> UpdateProductStock(UpdateProductStockRequest request)
        {
            return CreateActionResult(await productService.UpdateProductStockAsync(request));
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResult(await productService.DeleteAsync(id));
        }
    }
}
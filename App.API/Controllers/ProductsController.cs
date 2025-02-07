using App.Services;
using App.Services.Products;
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
        [HttpGet]
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
        [HttpPut]
        public async Task<IActionResult> Update(int id,UpdateProductsRequest request)
        {
            return CreateActionResult(await productService.UpdateAsync(id, request));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResult(await productService.DeleteAsync(id));
        }
    }
}
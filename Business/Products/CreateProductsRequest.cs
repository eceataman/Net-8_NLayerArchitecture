
using System.Threading.Tasks;

namespace App.Services.Products
{
    public record CreateProductsRequest(string Name, decimal Price, int Stock);
   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
   public class Product
    {
        public int Id{ get; set; }
        public string Name { get; set; } = default!;
        // int değere default olarak 0 atanır ama string oldugunda null değeri olmasın diye bu yazılır
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}

using App.Repositories.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    public class CreateProductRequestValidator:AbstractValidator<CreateProductsRequest>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductRequestValidator(IProductRepository productRepository) {
            _productRepository = productRepository;
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("ürün ismi gereklidir")
            //.NotNull().WithMessage("ürün ismi gereklidir");
            .Length(3, 10).WithMessage("minimum 3 maksşimum 10 karakter olmalıdır");
            //.Must(MustUniqueProductName).WithMessage("ürün ismi veritabanında bulunmaktadır");
            // integer ise not null ı kullanamazsın 
            //name null, price ve stock default olarak 0 değerindedir
            //Eğer bir değişken int ise, null olamaz ve varsayılan olarak 0 olur.
//✔ Eğer nullable(int?) olarak tanımlarsan, null olabilir ve .NotNull() validasyonu çalışabilir.
            //.NotNull() metodu referans türlerinde (string, object, class) çalışır.Price ve Stock → null olamaz çünkü int bir değer türüdür. Eğer bir değer atanmazsa default(int) = 0 olur.
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("ürün değeri 0 dan büyük olmalıdır");
            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("ürün stok değeri 1 ile 100 arasında olmalıdır");
        }
        //private bool MustUniqueProductName(string name)
        //{
        //    return !_productRepository.Where(X=> X.Name == name).Any();
        //}

    }
}

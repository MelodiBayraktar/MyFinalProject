// See https://aka.ms/new-console-template for more information

using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;

ProductManager productManager = new ProductManager(new EfProductDal());
var result = productManager.GetProductDetails();

if (result.Success)
{
    foreach (var product in result.Data)
    {
        Console.WriteLine(product.ProductName + " / " + product.CategoryName );
    }
}
else
{
    Console.WriteLine(result.Message);
}


/*
 ProductManager productManager = new ProductManager(new EfProductDal());

foreach (var product in productManager.GetByUnitPrice(10,30))
{
    Console.WriteLine(product.ProductName);
}
*/

using Core.DataAccess;
using Entities.Concrete;
using Entities.Dto;

namespace DataAccess.Abstract;


public interface IProductDal:IEntityRepository<Product>
{
    List<ProductDetailDto> GetProductDetails();
}
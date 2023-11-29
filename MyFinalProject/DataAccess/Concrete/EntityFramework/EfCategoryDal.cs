using System.Linq.Expressions;
using Core.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class EfCategoryDal:EfEntityRepositoryBase<Category, NortwindContext>,ICategoryDal
{
   
}
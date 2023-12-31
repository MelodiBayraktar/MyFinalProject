using System.ComponentModel.DataAnnotations;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto;
using FluentValidation;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Business.Concrete;

public class ProductManager:IProductService
{
    IProductDal _productDal;
   ICategoryService _categoryService;


   public ProductManager(IProductDal productDal, ICategoryService categoryService)
   {
       _productDal = productDal;
       _categoryService = categoryService;
   }

   public IDataResult<List<Product>> GetAll()
    {
        if (DateTime.Now.Hour == 01)
        {
            return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
        }
        else
        {
           return  new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed); 
        }
        
    }

    public IDataResult<List<Product>> GetAllByCategoryId(int id)
    {
        return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id)) ;
    }

    public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
    {
        return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max)) ;
    }

    public IDataResult<List<ProductDetailDto>> GetProductDetails()
    {
        if (DateTime.Now.Hour == 00)
        {
            return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
        }
        return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());

        
    }

    public IDataResult<Product> GetById(int productId)
    {
        return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
    }
   [ValidationAspect(typeof(ProductValidator))]
    public IResult Add(Product product)
    {
        IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
            CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());

        if (result != null) 
        {
            return result;
        }
        _productDal.Add(product);
        return new SuccessResult(Messages.ProductAdded);
                
    }

    public IResult Delete(Product product)
    {
        _productDal.Delete(product);
        return new SuccessResult(Messages.ProductDeleted);
    }
    [ValidationAspect(typeof(ProductValidator))]
    public IResult Update(Product product)
    {
        _productDal.Update(product);
        return new SuccessResult(Messages.ProductUpdated);
    }
    private IResult CheckIfProductCountOfCategoryCorrect(int  categoryId)
    {
        var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
        if (result >= 15)
        {
            return new ErrorResult(Messages.ProductCountOfCategoryError);
        }
        return new SuccessResult();
    }
    private IResult CheckIfProductNameExists(string productName)
    {
        var result = _productDal.GetAll(p => p.ProductName == productName).Any();
        if (result)
        {
            return new ErrorResult(Messages.ProductNameAlreadyExists);
        }
        return new SuccessResult();
    }
    private IResult CheckIfCategoryLimitExceded() 
    {
        var result = _categoryService.GetAll();
        if (result.Data.Count>15)
        {
            return new ErrorResult(Messages.CategoryLimitExceded);
        }
        return new SuccessResult();
    }   
}
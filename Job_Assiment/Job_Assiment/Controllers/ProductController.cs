using Assiment.core.Models;
using Assiment.DTO;
using Assiment.EF.DTO;
using Assiment.EF.Services;
using ECommerce.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assiment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductService _productService;
        UnitOfWork _unitOfWork;

        public ProductController(ProductService productService,UnitOfWork unitOfWork)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("AddNewProduct")]
        public void AddNewProduct([FromForm]ProductDto productDto)
        {
            _productService.Add(productDto);
            _unitOfWork.CommitChanges();

        }

        [HttpGet("GetProductById")]
        public Product GetProductById(int id)
        {
            Product product = _productService.GetByID(id);
            _unitOfWork.CommitChanges();
            return product;

        }


        [HttpPut("UpdateProduct")]
        public void UpdateProduct([FromForm]UpdateProductDto product,[FromForm] params string[] properties)
        {
             _productService.Update(product,properties);            
            _unitOfWork.CommitChanges();
        }

        [HttpPut("UpdateProductImage")]
        public void UpdateProductImage([FromForm]UpdataProductImage updataProductImage)
        {
            _productService.UpdateImage(updataProductImage);
            _unitOfWork.CommitChanges();
        }



        [HttpGet("GetAllProducts")]
         public PagingResult<Product> GetAllProducts(int pageNumber, int pageSize)
         {
            PagingResult<Product> pagingResult=_productService.GetAll(pageNumber,pageSize);
            return pagingResult;

         }

        [HttpGet("DynamicSearch")]
        public IEnumerable<Product> DynamicSearch(string searchproperty)
        {
            IEnumerable<Product> products =_productService.Search(searchproperty);
            return products;
            
        }


        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
             _productService.Delete(id);
            _unitOfWork.CommitChanges();

            return Ok("Delete Success");

        }

    }
}

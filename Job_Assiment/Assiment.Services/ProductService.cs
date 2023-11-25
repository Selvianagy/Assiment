using Assiment.core.Interfaces;
using Assiment.core.Models;
using Assiment.DTO;
using Assiment.EF.DTO;
using ECommerce.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assiment.EF.Services
{
    public class ProductService
    {
        IGenericRepository<Product> _repository;
        UnitOfWork _unitOfWork;

        public ProductService(IGenericRepository<Product> repository,UnitOfWork unitOfWork) {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public PagingResult<Product> GetAll(int pageNumber, int pageSize)
        {
            int totalItems = _repository.GetCount();
            List<Product> products = _repository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            PagingResult<Product> pagingResult= new PagingResult<Product>();
            pagingResult.TotalItems = totalItems;
            pagingResult.Items = products;
            pagingResult.pageSize = pageSize;
            pagingResult.pageNumber= pageNumber;

            return pagingResult;
        }

        public IEnumerable<Product> Get(Expression<Func<Product, bool>> expression)
        {
           List<Product> products = _repository.Get(expression).ToList();
            return products;
        }

        public Product GetByID(int id)
        {
            Product product= _repository.GetByID(id);
            return product;
        }

        public Product Add(ProductDto productDto)
        {
            Product newproduct = new Product();
            newproduct.Quentity = productDto.Quentity;
            newproduct.Price=productDto.Price;
            newproduct.Name = productDto.Name;

            using var dataStream = new MemoryStream();
            productDto.image.CopyTo(dataStream);
            newproduct.image = dataStream.ToArray();

            Product product= _repository.Add(newproduct);

            _unitOfWork.SaveChanges();
            return product;
        }

        public void Update(UpdateProductDto updateproduct, params string[] properties)
        {
            Product product = new Product();
            product.Id = updateproduct.ProductId;
            product.Quentity = updateproduct.Quentity;
            product.Price = updateproduct.Price;
            product.Name = updateproduct.Name;

            _repository.Update(product,properties);
            _unitOfWork.SaveChanges();

        }
        public void UpdateImage(UpdataProductImage updateproductimage)
        {
            Product product = new Product();

            using var dataStream = new MemoryStream();
            updateproductimage.image.CopyTo(dataStream);
            product.image = dataStream.ToArray();
            product.Id=updateproductimage.ProductId;

            _repository.Update(product, nameof(Product.image));

            _unitOfWork.SaveChanges();


        }
        public void Delete(int id)
        {
            Product product=_repository.GetByID(id);
            product.IsDeleted = true;
            _repository.Delete(product);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<Product> Search(string searchProperty)
        {
            IEnumerable<Product> products = _repository.Get(p => p.Name.Contains(searchProperty)||
            p.Price.ToString().Contains(searchProperty))
            .ToList();

            return products;
        }

    }
}

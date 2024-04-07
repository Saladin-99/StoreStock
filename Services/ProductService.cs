using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StoreStock.Data;
using StoreStock.Models;

namespace StoreStock.Services
{
    public class ProductService(MyDbContext context)
    {
        private readonly MyDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public void CreateProduct(Product product)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(product);

                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while creating the product.", ex);
            }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                var product = _context.Products.ToList();
                if (product.Count == 0)
                {
                    throw new EntityNotFoundException($"No products created yet.");
                }
                
                return product;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while retrieving products.", ex);
            }
        }

        public Product GetProductById(int id)
        {
            try
            {
                var product = _context.Products.Find(id) ?? throw new EntityNotFoundException($"Product with ID {id} not found.");
                return product;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while retrieving the product.", ex);
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(product);
                var oldProduct = _context.Products.Find(product.Id) ?? throw new EntityNotFoundException($"Product not found.");
                oldProduct.Name = product.Name;
                oldProduct.Price = product.Price;
                oldProduct.Description = product.Description;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while updating the product.", ex);
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                var product = _context.Products.Find(id) ?? throw new EntityNotFoundException($"Product with ID {id} not found.");
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while deleting the product.", ex);
            }
        }
    }

    // Custom exception for entity not found
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }
        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Custom exception for service-related errors
    public class ServiceException : Exception
    {
        public ServiceException() { }
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}

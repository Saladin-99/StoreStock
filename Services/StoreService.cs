using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using StoreStock.Data;
using StoreStock.Models;

namespace StoreStock.Services
{
    public class StoreService(MyDbContext context)
    {
        private readonly MyDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public void CreateStore(Store store)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(store);

                _context.Stores.Add(store);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while creating the store.", ex);
            }
        }

        public List<Store> GetAllStores()
        {
            try
            {
                var stores = _context.Stores.ToList();
                if (stores.Count == 0)
                {
                    throw new EntityNotFoundException("No stores found.");
                }
                
                return stores;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while retrieving stores. " + ex.Message, ex);
            }
        }

        public Store GetStoreById(int id)
        {
            try
            {
                var store = _context.Stores.Find(id) ?? throw new EntityNotFoundException($"Store with ID {id} not found.");
                return store;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while retrieving the store."  + ex.Message, ex);
            }
        }

        public void UpdateStore(Store store)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(store);

                var oldStore = _context.Stores.Find(store.Id) ?? throw new EntityNotFoundException($"Store not found.");
                
                oldStore.Name = store.Name;
                oldStore.Address = store.Address;
                // Update other properties as needed
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while updating the store. " + ex.Message, ex);
            }
        }

        public void DeleteStore(int id)
        {
            try
            {
                var store = _context.Stores.Find(id) ?? throw new EntityNotFoundException($"Store with ID {id} not found.");
                
                _context.Stores.Remove(store);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while deleting the store. " + ex.Message, ex);
            }
        }

        public List<Product> GetProductsInStock(int storeId)
        {
            try
            {
                var productsInStock = _context.StockItems
                    .Where(si => si.StoreId == storeId)
                    .Include(si => si.Product)
                    .Select(si => si.Product)
                    .ToList();
                if (productsInStock.Count == 0)
                {
                    throw new EntityNotFoundException("No products in stock.");
                }
                return productsInStock;
                
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while retrieving products in stock for the store. " + ex.Message, ex);
            }
        }

        public void AddStockItem(StockItem stockItem)
        {
            try
            {
                var store = _context.Stores.Include(s => s.StockItems).FirstOrDefault(s => s.Id == stockItem.StoreId) ?? throw new EntityNotFoundException($"Store with ID {stockItem.StoreId} not found.");

                // Ensure stockItems collection is initialized
                store.StockItems ??= [];

                store.StockItems.Add(stockItem);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while adding a stock item to the store. " + ex.Message, ex);
            }

        }

        public void UpdateStockItem(StockItem stockItem)
        {
            try
            {
                var store = _context.Stores.Include(s => s.StockItems).FirstOrDefault(s => s.Id == stockItem.StoreId) ?? throw new EntityNotFoundException($"Store with ID {stockItem.StoreId} not found.");
                ArgumentNullException.ThrowIfNull(store.StockItems);
                var existingStockItem = store.StockItems.FirstOrDefault(si => si.ProductId == stockItem.ProductId) ?? throw new EntityNotFoundException($"Stock item not found in the store.");
                existingStockItem.Quantity = stockItem.Quantity;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while updating a stock item in the store. " + ex.Message, ex);
            }
        }
    }
}

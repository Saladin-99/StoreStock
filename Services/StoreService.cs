using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using StoreStock.Data;
using StoreStock.Models;

namespace StoreStock.Services
{
    public class StoreService
    {
        private readonly MyDbContext _context;

        public StoreService(MyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void CreateStore(Store store)
        {
            try
            {
                if (store == null)
                {
                    throw new ArgumentNullException(nameof(store));
                }

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
                if (stores == null || stores.Count == 0)
                {
                    throw new EntityNotFoundException("No stores found.");
                }
                
                return stores;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while retrieving stores.", ex);
            }
        }

        public Store GetStoreById(int id)
        {
            try
            {
                var store = _context.Stores.Find(id);
                if (store == null)
                {
                    throw new EntityNotFoundException($"Store with ID {id} not found.");
                }
                return store;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while retrieving the store.", ex);
            }
        }

        public void UpdateStore(Store store)
        {
            try
            {
                if (store == null)
                {
                    throw new ArgumentNullException(nameof(store));
                }
                var oldStore = _context.Stores.Find(store.Id);

                if (oldStore == null)
                {
                    throw new EntityNotFoundException($"Store not found.");
                }

                oldStore.Name = store.Name;
                oldStore.Address = store.Address;
                // Update other properties as needed
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while updating the store.", ex);
            }
        }

        public void DeleteStore(int id)
        {
            try
            {
                var store = _context.Stores.Find(id);
                if (store == null)
                {
                    throw new EntityNotFoundException($"Store with ID {id} not found.");
                }

                _context.Stores.Remove(store);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while deleting the store.", ex);
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
                
                return productsInStock;
                
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while retrieving products in stock for the store.", ex);
            }
        }

        public void AddStockItem(StockItem stockItem)
        {
            try
            {
                var store = _context.Stores.Include(s => s.StockItems).FirstOrDefault(s => s.Id == stockItem.StoreId);
                if (store == null)
                {
                    throw new EntityNotFoundException($"Store with ID {stockItem.StoreId} not found.");
                }

                // Ensure stockItems collection is initialized
                if (store.StockItems == null)
                {
                    store.StockItems = new List<StockItem>();
                }

                store.StockItems.Add(stockItem);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while adding a stock item to the store.", ex);
            }

        }

        public void UpdateStockItem(StockItem stockItem)
        {
            try
            {
                var store = _context.Stores.Include(s => s.StockItems).FirstOrDefault(s => s.Id == stockItem.StoreId);
                if (store == null)
                {
                    throw new EntityNotFoundException($"Store with ID {stockItem.StoreId} not found.");
                }

                var existingStockItem = store.StockItems.FirstOrDefault(si => si.ProductId == stockItem.ProductId);
                if (existingStockItem == null)
                {
                    throw new EntityNotFoundException($"Stock item not found in the store.");
                }

                existingStockItem.Quantity = stockItem.Quantity;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while updating a stock item in the store.", ex);
            }
        }
    }
}

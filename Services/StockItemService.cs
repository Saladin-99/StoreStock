using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreStock.Data;
using StoreStock.Models;

namespace StoreStock.Services
{
    public class StockItemService(MyDbContext context, StoreService storeService)
    {
        private readonly MyDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly StoreService _storeService = storeService ?? throw new ArgumentNullException(nameof(storeService));

        public List<StockItem> GetAllStockItems()
    {
    try
    {
        var stockItems = _context.StockItems
            .Include(si => si.Store)
            .Include(si => si.Product)
            .ToList();

        if (stockItems.Count == 0)
        {
            throw new EntityNotFoundException("No stock items found.");
        }

        return stockItems;
    }
    catch (Exception ex)
    {
        // Log or handle the exception
        throw new ServiceException("Error occurred while retrieving stock items. " + ex.Message , ex);
    }
}

    public StockItem GetStockItemById(int storeId, int productId)
    {
    try
    {
        var stockItem = _context.StockItems
            .Include(si => si.Store)
            .Include(si => si.Product)
            .FirstOrDefault(si => si.StoreId == storeId && si.ProductId == productId);
                return stockItem;
    }
    catch (Exception ex)
    {
        // Log or handle the exception
        throw new ServiceException("Error occurred while retrieving stock item by ID. " + ex.Message , ex);
    }
}


        public void CreateItem(int storeId, int productId, int quantity)
        {
            try
            {
               
                var store = _context.Stores.Find(storeId) ?? throw new EntityNotFoundException("Store not Found.");
                var product = _context.Products.Find(productId) ?? throw new EntityNotFoundException("Product not Found.");

                // Create a new stock item
                var newStockItem = new StockItem
                {
                    StoreId = storeId,
                    ProductId = productId,
                    Quantity = quantity,
                    Store = store,
                    Product = product
                };
                _context.StockItems.Add(newStockItem);
                _storeService.AddStockItem(newStockItem);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while creating the stock item. " + ex.Message , ex);
            }
        }

        public void UpdateItemQuantity(int storeId, int productId, int quantity)
        {
            try
            {
                var existingStockItem = GetStockItemById(storeId, productId) ?? throw new EntityNotFoundException("Stock item not found.");
                // Update the quantity of the existing stock item
                existingStockItem.Quantity += quantity;
                _storeService.UpdateStockItem(existingStockItem);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while updating stock item quantity. " + ex.Message , ex);
            }
        }

        public void DeleteStockItem(int storeId, int productId)
        {
            try
            {
                var stockItemToDelete = _context.StockItems.FirstOrDefault(si => si.StoreId == storeId && si.ProductId == productId) ?? throw new EntityNotFoundException("Stock item not found.");
                _context.StockItems.Remove(stockItemToDelete);

                var store = _context.Stores.FirstOrDefault(s => s.Id == storeId) ?? throw new EntityNotFoundException("Store not found.");
                store.StockItems?.Remove(stockItemToDelete);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while deleting stock item. " + ex.Message, ex);
            }
        }


        public void DeleteStock(int storeId)
        {
            try
            {
                var stockItemsToDelete = _context.StockItems.Where(si => si.StoreId == storeId).ToList();
                if (stockItemsToDelete.Count != 0)
                {

                    _context.StockItems.RemoveRange(stockItemsToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while deleting stock items. " + ex.Message, ex);
            }
        }

    }
}

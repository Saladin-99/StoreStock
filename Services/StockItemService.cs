using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreStock.Data;
using StoreStock.Models;

namespace StoreStock.Services
{
    public class StockItemService
    {
        private readonly MyDbContext _context;
        private readonly StoreService _storeService;

        public StockItemService(MyDbContext context, StoreService storeService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _storeService = storeService?? throw new ArgumentNullException(nameof(storeService));
        }

        public void CreateItemAndAdd(int storeId, int productId, int quantity)
        {
            try
            {
                var existingStockItem = _context.StockItems
                    .FirstOrDefault(si => si.StoreId == storeId && si.ProductId == productId);
                var store = _context.Stores.Find(storeId);
                var product = _context.Products.Find(productId);
                if (store == null)
                {
                    throw new EntityNotFoundException("Store not Found.");
                }
                if (product == null)
                {
                    throw new EntityNotFoundException("Product not Found.");
                }

                if (existingStockItem != null)
                {
                    // StockItem already exists, increase quantity
                    existingStockItem.Quantity += quantity;
                    _storeService.UpdateStockItem(existingStockItem);
                    
                }
                else
                {
                    // StockItem doesn't exist, create a new one
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
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new ServiceException("Error occurred while creating or updating the stock item.", ex);
            }
        }
    }
}

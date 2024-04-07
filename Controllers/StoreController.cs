using Microsoft.AspNetCore.Mvc;
using StoreStock.Models;
using StoreStock.Services;
using System;

namespace StoreStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController(StoreService storeService, StockItemService stockItemService) : ControllerBase
    {
        private readonly StoreService _storeService = storeService;

        private readonly StockItemService _stockItemService = stockItemService;

        [HttpGet]
        public IActionResult GetAllStores()
        {
            try
            {
                var stores = _storeService.GetAllStores();
                return Ok(stores);
            }
            catch (Exception ex)
            {
                if(ex.InnerException is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetStoreById(int id)
        {
            try
            {
                var store = _storeService.GetStoreById(id);
                return Ok(store);
            }
            catch (Exception ex)
            {
                if(ex.InnerException is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateStore(Store store)
        {
            try
            {
                _storeService.CreateStore(store);
                return CreatedAtAction(nameof(GetStoreById), new { id = store.Id }, store);
            }
            catch (Exception ex)
            {
                if(ex.InnerException is ArgumentNullException)
                {return BadRequest(ex.Message);}
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateStore(Store store)
        {
            try
            {
                _storeService.UpdateStore(store);
                return Ok("Store updated successfully!");
            }
            catch (Exception ex)
            {
                if(ex.InnerException is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                else if(ex.InnerException is ArgumentNullException)
                {return BadRequest(ex.Message);}
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStore(int id)
        {
            try
            {
                _storeService.DeleteStore(id);
                return Ok("Store deleted successfully!");
            }
            catch (Exception ex)
            {
                if(ex.InnerException is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{storeId}/products")]
        public IActionResult GetStockOfStore(int storeId)
        {
            try
            {
                var products = _storeService.GetStock(storeId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                if(ex.InnerException is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("{storeId}/addStock")]
        public IActionResult AddStockToStore(int storeId, StockUpdateRequest stock)
        {
            try
            {
                Console.Write("aa"+stock.ProductId+"bb"+storeId);
                StockItem? stockItem = _stockItemService.GetStockItemById(storeId,stock.ProductId);
                if(stockItem == null)
                {
                    _stockItemService.CreateItem(storeId,stock.ProductId,stock.Quantity);
                }
                else
                {
                    _stockItemService.UpdateItemQuantity(storeId,stock.ProductId,stock.Quantity);
                }
                return Ok("Stock added to store successfully!");
            }
            catch (Exception ex)
            {
                if(ex.InnerException is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }

    public class StockUpdateRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}

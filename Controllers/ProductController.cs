using Microsoft.AspNetCore.Mvc;
using StoreStock.Models;
using StoreStock.Services;
using System;
using System.Runtime.InteropServices;

namespace StoreStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            Console.Write("all");
            try
            {
                var products = _productService.GetAllProducts();
                return Ok(products);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            try
            {   
                var product = _productService.GetProductById(id);
                return Ok(product);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    return NotFound();
                }
                else if(product.Name == null)
                {
                    return BadRequest("Name must have a value.");
                }
                _productService.CreateProduct(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {

            try
            {
                _productService.UpdateProduct(product);
                return Ok("Product updated successfully!");
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return Ok("Product deleted successfully!");
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }
    }
}

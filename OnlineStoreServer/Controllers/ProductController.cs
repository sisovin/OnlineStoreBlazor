﻿using Microsoft.AspNetCore.Mvc;
using OnlineStoreServer.Repositories;
using OnlineStoreSharedLibrary.Models;
using OnlineStoreSharedLibrary.Responses;

namespace OnlineStoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts(bool featured)
        {
            var products = await productService.GetAllProducts(featured);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddProduct(Product model)
        {
            if (model is null) return BadRequest("Model is null.");
            var response = await productService.AddProduct(model);
            return Ok(response);
        }
    }
}

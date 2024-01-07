using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductCRUD.Data;
using ProductCRUD.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        // Execute non-composable SQL query to get all products
        var products = _context.Products
            .FromSqlRaw("EXEC dbo.GetProducts")
            .ToList();

        if (products == null || products.Count == 0)
        {
            // If no products are found, return NotFound or an appropriate response
            return NotFound();
        }

        return Ok(products);
    }


    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var productIdParameter = new SqlParameter("@ProductId", id);

        // Execute non-composable SQL query and perform composition on the client side
        var product = _context.Products
            .FromSqlRaw("EXEC dbo.GetProductById @ProductId", productIdParameter)
            .AsEnumerable()
            .FirstOrDefault();

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        var productIdParameter = new SqlParameter("@ProductId", id);

        // Execute non-composable SQL query and perform composition on the client side
        var existingProduct = _context.Products
            .FromSqlRaw("EXEC dbo.GetProductById @ProductId", productIdParameter)
            .AsEnumerable()
            .FirstOrDefault();

        if (existingProduct == null)
        {
            return NotFound();
        }

        // Update properties of the existing product
        existingProduct.Name = product.Name;
        existingProduct.Quantity = product.Quantity;
        existingProduct.Category = product.Category;
        existingProduct.UnitPrice = product.UnitPrice;

        // Save changes to the database
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var productIdParameter = new SqlParameter("@ProductId", id);

        // Execute non-composable SQL query to delete a product by ID
        var rowsAffected = _context.Database
            .ExecuteSqlRaw("EXEC dbo.DeleteProduct @ProductId", productIdParameter);

        if (rowsAffected == 0)
        {
            // If no rows were affected, the product with the specified ID was not found
            return NotFound();
        }

        return NoContent();
    }


}

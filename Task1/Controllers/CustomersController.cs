using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task1.Data;
using Task1.Dtos;
using Task1.Models;

namespace Task1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _db;

    public CustomersController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        var customers = await _db.Customers.AsNoTracking().ToListAsync();
        return Ok(customers);
    }

    // GET: api/customers/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await _db.Customers.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer is null) return NotFound();

        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<Customer>> Create(CreateCustomerRequest req)
    {

        var customer = new Customer
        {
            Name = req.Name.Trim(),
            Email = req.Email.Trim(),
            Phone = req.Phone?.Trim(),
            DateOfBirth = req.DateOfBirth,
            IsActive = req.IsActive ?? true,
            CreatedAt = DateTime.UtcNow
        };

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    // PUT: api/customers/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Customer>> Update(int id, UpdateCustomerRequest req)
    {

        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (customer is null) return NotFound();

        customer.Name = req.Name.Trim();
        customer.Email = req.Email.Trim();
        customer.Phone = req.Phone?.Trim();
        customer.DateOfBirth = req.DateOfBirth;
        customer.IsActive = req.IsActive;

        await _db.SaveChangesAsync();

        return Ok(customer);
    }
}

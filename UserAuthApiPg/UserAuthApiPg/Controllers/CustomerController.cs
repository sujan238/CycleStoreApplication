using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApiPg.Data;
using UserAuthApiPg.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserAuthApiPg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // GET: api/<ValuesController>
        private readonly AuthDbContext _context;
        public CustomersController(AuthDbContext context) {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GteAllCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
            return Ok(customer);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer newCustomer)
        {
            await _context.Customers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = newCustomer.CustomerId }, newCustomer);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using UserAuthApiPg.Data;
using UserAuthApiPg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserAuthApiPg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleBrandController : ControllerBase
    {
        private readonly AuthDbContext _context;

        public CycleBrandController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: api/cyclebrands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CycleBrand>>> GetCycleBrands()
        {
            return await _context.CycleBrands.ToListAsync();
        }

        // GET: api/cyclebrands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CycleBrand>> GetCycleBrand(int id)
        {
            var cycleBrand = await _context.CycleBrands.FindAsync(id);

            if (cycleBrand == null)
            {
                return NotFound();
            }

            return cycleBrand;
        }

        // POST: api/cyclebrands
        [HttpPost]
        public async Task<ActionResult<CycleBrand>> CreateCycleBrand(CycleBrand cycleBrand)
        {
            if (string.IsNullOrEmpty(cycleBrand.BrandName))
            {
                return BadRequest("BrandName is required.");
            }

            _context.CycleBrands.Add(cycleBrand);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCycleBrand), new { id = cycleBrand.BrandId }, cycleBrand);
        }

        // PUT: api/cyclebrands/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCycleBrand(int id, CycleBrand cycleBrand)
        {
            if (id != cycleBrand.BrandId)
            {
                return BadRequest();
            }

            _context.Entry(cycleBrand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CycleBrandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/cyclebrands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCycleBrand(int id)
        {
            var cycleBrand = await _context.CycleBrands.FindAsync(id);
            if (cycleBrand == null)
            {
                return NotFound();
            }

            _context.CycleBrands.Remove(cycleBrand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CycleBrandExists(int id)
        {
            return _context.CycleBrands.Any(e => e.BrandId == id);
        }
    }
}
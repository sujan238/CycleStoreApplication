using UserAuthApiPg.Data;
using UserAuthApiPg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserAuthApiPg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleTypeController : ControllerBase
    {
        private readonly AuthDbContext _context;

        public CycleTypeController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: api/cycletypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CycleType>>> GetCycleTypes()
        {
            return await _context.CycleTypes.ToListAsync();
        }

        // GET: api/cycletypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CycleType>> GetCycleType(int id)
        {
            var cycleType = await _context.CycleTypes.FindAsync(id);

            if (cycleType == null)
            {
                return NotFound();
            }

            return cycleType;
        }

        // POST: api/cycletypes
        [HttpPost]
        public async Task<ActionResult<CycleType>> CreateCycleType(CycleType cycleType)
        {
            if (string.IsNullOrEmpty(cycleType.TypeName))
            {
                return BadRequest("TypeName is required.");
            }

            _context.CycleTypes.Add(cycleType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCycleType), new { id = cycleType.TypeId }, cycleType);
        }

        // PUT: api/cycletypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCycleType(int id, CycleType cycleType)
        {
            if (id != cycleType.TypeId)
            {
                return BadRequest();
            }

            _context.Entry(cycleType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CycleTypeExists(id))
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

        // DELETE: api/cycletypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCycleType(int id)
        {
            var cycleType = await _context.CycleTypes.FindAsync(id);
            if (cycleType == null)
            {
                return NotFound();
            }

            _context.CycleTypes.Remove(cycleType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CycleTypeExists(int id)
        {
            return _context.CycleTypes.Any(e => e.TypeId == id);
        }
    }
}
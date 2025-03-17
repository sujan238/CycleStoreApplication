using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApiPg.Data;
using UserAuthApiPg.Models.Dtos;

namespace UserAuthApiPg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "OwnerOnly")]
    public class CycleController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly IMapper _mapper;

        public CycleController(AuthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        

        [HttpGet]
        public async Task<ActionResult<List<CycleDto>>> GetCycles()
        {
            var cycles = await _context.Cycles
                .Include(c => c.Brand)
                .Include(c => c.Type)
                .Include(c => c.Inventories)
                .AsNoTracking()
                .ToListAsync();

            return Ok(_mapper.Map<List<CycleDto>>(cycles));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CycleDto>> GetCycle(int id)
        {
            var cycle = await _context.Cycles
                .Include(c => c.Brand)
                .Include(c => c.Type)
                .Include(c => c.Inventories)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CycleId == id);

            if (cycle == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CycleDto>(cycle));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCycle(int id, [FromBody] CycleUpdateDto cycleUpdateDto)
        {
            if (cycleUpdateDto == null || cycleUpdateDto.CycleId != id)
            {
                return BadRequest("Invalid cycle data.");
            }

            var cycle = await _context.Cycles.FindAsync(id);
            if (cycle == null)
            {
                return NotFound();
            }

            _mapper.Map(cycleUpdateDto, cycle);
            await _context.SaveChangesAsync();

            var updatedCycle = await _context.Cycles
                .Include(c => c.Brand)
                .Include(c => c.Type)
                .Include(c => c.Inventories)
                .FirstOrDefaultAsync(c => c.CycleId == id);
            var cycleDto = _mapper.Map<CycleDto>(updatedCycle);

            return Ok(cycleDto);
        }

        [HttpGet("inventories/{cycleId}")]
        public async Task<ActionResult<List<InventoryDto>>> GetInventoriesForCycle(int cycleId)
        {
            var inventories = await _context.Inventories
                .Where(i => i.CycleId == cycleId)
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<List<InventoryDto>>(inventories));
        }

        [HttpPut("inventory/{inventoryId}")]
        public async Task<IActionResult> UpdateInventory(int inventoryId, [FromBody] InventoryDto inventoryDto)
        {
            if (inventoryDto == null || inventoryDto.InventoryId != inventoryId)
            {
                return BadRequest("Invalid inventory data.");
            }

            var inventory = await _context.Inventories.FindAsync(inventoryId);
            if (inventory == null)
            {
                return NotFound();
            }

            _mapper.Map(inventoryDto, inventory);
            await _context.SaveChangesAsync();

            var cycle = await _context.Cycles
                .Include(c => c.Inventories)
                .FirstOrDefaultAsync(c => c.CycleId == inventory.CycleId);
            var cycleDto = _mapper.Map<CycleDto>(cycle);

            return Ok(cycleDto);
        }
    }
}
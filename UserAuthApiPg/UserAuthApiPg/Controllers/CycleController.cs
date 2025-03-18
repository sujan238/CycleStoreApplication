using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApiPg.Data;
using UserAuthApiPg.Models;
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

        [HttpPost]
        public async Task<ActionResult<CycleDto>> AddCycle([FromBody] CycleUpdateDto cycleUpdateDto)
        {
            if (cycleUpdateDto == null)
            {
                return BadRequest("Invalid cycle data.");
            }

            var cycle = _mapper.Map<Cycle>(cycleUpdateDto);
            cycle.DeliveryCharges = 5.00m; // Default value
            cycle.Color = "Black"; // Default value
            cycle.Size = "Standard"; // Default value
            cycle.Inventories = new List<Inventory>(); // Initialize empty inventory collection

            _context.Cycles.Add(cycle);
            await _context.SaveChangesAsync();

            var addedCycle = await _context.Cycles
                .Include(c => c.Brand)
                .Include(c => c.Type)
                .Include(c => c.Inventories)
                .FirstOrDefaultAsync(c => c.CycleId == cycle.CycleId);
            var cycleDto = _mapper.Map<CycleDto>(addedCycle);

            return CreatedAtAction(nameof(GetCycle), new { id = cycle.CycleId }, cycleDto);
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

        [HttpPost("inventory")]
        public async Task<ActionResult<InventoryDto>> AddInventory([FromBody] InventoryDto inventoryDto)
        {
            if (inventoryDto == null)
            {
                return BadRequest("Invalid inventory data.");
            }

            var inventory = _mapper.Map<Inventory>(inventoryDto);
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            var cycle = await _context.Cycles
                .Include(c => c.Inventories)
                .FirstOrDefaultAsync(c => c.CycleId == inventory.CycleId);
            var cycleDto = _mapper.Map<CycleDto>(cycle);

            return CreatedAtAction(nameof(GetInventoriesForCycle), new { cycleId = inventory.CycleId }, cycleDto);
        }
    }
}
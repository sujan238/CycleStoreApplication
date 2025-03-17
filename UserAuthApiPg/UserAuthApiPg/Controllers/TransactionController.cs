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
    public class TransactionsController : ControllerBase
    {
        private readonly AuthDbContext _context;

        public TransactionsController(AuthDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequest request)
        {
            if (request == null || request.CustomerId <= 0 || request.OrderDetails == null || !request.OrderDetails.Any())
            {
                return BadRequest("Invalid transaction request.");
            }

            var customer = await _context.Customers.FindAsync(request.CustomerId);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var transaction = new Transaction
            {
                CustomerId = request.CustomerId,
                EmployeeId = 1, // Replace with authenticated employee ID
                PurchaseDate = DateTime.UtcNow,
                TotalAmount = 0 // Calculated from OrderDetails
            };

            foreach (var orderDetailRequest in request.OrderDetails)
            {
                var cycle = await _context.Cycles
                    .Include(c => c.Inventories)
                    .FirstOrDefaultAsync(c => c.CycleId == orderDetailRequest.CycleId);

                if (cycle == null)
                {
                    return NotFound($"Cycle with ID {orderDetailRequest.CycleId} not found.");
                }

                // Check inventory stock for the requested color and store (assuming multi-store)
                var inventory = cycle.Inventories
                    .FirstOrDefault(i => i.Color == orderDetailRequest.CustomColor && i.StockQuantity >= orderDetailRequest.Quantity);

                if (inventory == null)
                {
                    return BadRequest($"Insufficient stock for Cycle ID {orderDetailRequest.CycleId} with color {orderDetailRequest.CustomColor}.");
                }

                var orderDetail = new OrderDetail
                {
                    TransactionId = transaction.TransactionId,
                    CycleId = orderDetailRequest.CycleId,
                    Quantity = orderDetailRequest.Quantity,
                    UnitPrice = cycle.Price,
                    CustomColor = orderDetailRequest.CustomColor,
                    Subtotal = cycle.Price * orderDetailRequest.Quantity
                };

                transaction.OrderDetails.Add(orderDetail);
                transaction.TotalAmount += orderDetail.Subtotal;

                // Update inventory
                inventory.StockQuantity -= orderDetailRequest.Quantity;
            }

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Transaction completed successfully", transactionId = transaction.TransactionId });
        }

        [HttpPost("{cycleId}/buy")]
        public async Task<IActionResult> BuyCycle(int cycleId, [FromBody] TransactionRequest request)
        {
            request.OrderDetails = new List<OrderDetailRequest> { new OrderDetailRequest { CycleId = cycleId, Quantity = request.Quantity ?? 1, CustomColor = request.CustomColor } };
            return await CreateTransaction(request);
        }
    }

    public class TransactionRequest
    {
        public int CustomerId { get; set; }
        public List<OrderDetailRequest> OrderDetails { get; set; } = new List<OrderDetailRequest>();
        public string? CustomColor { get; set; } // For single-cycle buy
        public int? Quantity { get; set; } = 1;
    }

    public class OrderDetailRequest
    {
        public int CycleId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? CustomColor { get; set; }
    }
}
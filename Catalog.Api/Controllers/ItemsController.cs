using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Dtos;
using Catalog.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers {

	[ApiController]
	[Route("items")]
	public class ItemsController : ControllerBase {
		private readonly CatalogDBContext _context;
		private readonly ILogger<ItemsController> _logger;
		//private readonly IItemsRepository _repository;

		public ItemsController(CatalogDBContext context, ILogger<ItemsController> logger) {
			this._context = context;
			this._logger = logger;
		}

		// GET /items
		[HttpGet]
		public async Task<IEnumerable<Item>> GetItemsAsync() {
			try {
				var items = await _context.Items.ToListAsync();
				_logger.LogInformation($"Retrieved {items.Count()} items");
				return items;
			} catch (Exception ex) {
				_logger.LogError(ex, "Stopped program because of exception");
				throw;
			}
		}

		// GET /items/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ItemDto>> GetItemAsync(int id) {
			try {
				var item = await _context.Items.FindAsync(id);

				if (item == null) {
					_logger.LogWarning($"item is null");
					return NotFound();
				}

				return item.AsDto();
			} catch (Exception ex) {
				_logger.LogError(ex, "Stopped program because of exception");
				throw;
			}
		}

		// POST /items
		[HttpPost]
		public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto) {
			try {
				Item item = new() {
					Name = itemDto.Name,
					ValidityStart = itemDto.ValidityStart,
					ValidityEnd = itemDto.ValidityEnd
				};
				
				if (item.ValidityStart >= item.ValidityEnd) {
					_logger.LogWarning("Validity Start date can't be highter than Validity End date");
					return BadRequest();
				}

				await _context.Items.AddAsync(item);

				await _context.SaveChangesAsync();

				return item.AsDto();
			} catch (Exception ex) {
				_logger.LogError(ex, "Stopped program because of exception");
				throw;
			}
		}

		// DELETE /items/{id}
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteItemAsync(int id) {
			try {
				var existingItem = await _context.Items.FindAsync(id);

				if (existingItem == null) {
					_logger.LogWarning($"item not found for deletion");
					return NotFound();
				}

				_context.Items.Remove(existingItem);

				await _context.SaveChangesAsync();

				return NoContent();
			} catch (Exception ex) {
				_logger.LogError(ex, "Stopped program because of exception");
				throw;
			}
		}
	}
}
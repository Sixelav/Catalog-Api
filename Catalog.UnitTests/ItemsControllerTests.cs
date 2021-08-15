using System;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests {
	public class ItemsControllerTests {
		private readonly Random rand = new();
		private readonly Mock<ILogger<ItemsController>> loggerStub = new();

		[Fact]
		public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound() {
			var options = new DbContextOptionsBuilder<CatalogDBContext>().UseInMemoryDatabase(databaseName: "GetItemAsync_WithUnexistingItem_ReturnsNotFound").Options;

			using (var context = new CatalogDBContext(options)) {
				context.Items.Add(new Item { Id = 1, Name = "Objet 1", ValidityStart = DateTime.Now, ValidityEnd = DateTime.Now.AddYears(3) });
				context.Items.Add(new Item { Id = 2, Name = "Objet 2", ValidityStart = DateTime.Now, ValidityEnd = DateTime.Now.AddYears(5) });
				context.Items.Add(new Item { Id = 3, Name = "Objet 3", ValidityStart = DateTime.Now, ValidityEnd = DateTime.Now.AddYears(8) });
				context.SaveChanges();
			}

			using (var context = new CatalogDBContext(options)) {
				ItemsController itemsController = new ItemsController(context, loggerStub.Object);
				var result = await itemsController.GetItemAsync(4);


				result.Result.Should().BeOfType<NotFoundResult>();
			}

			await Task.CompletedTask;
		}

		[Fact]
		public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem() {
			var options = new DbContextOptionsBuilder<CatalogDBContext>().UseInMemoryDatabase(databaseName: "GetItemAsync_WithExistingItem_ReturnsExpectedItem").Options;

			var expectedItem = CreateRandomItem();

			using (var context = new CatalogDBContext(options)) {
				context.Items.Add(expectedItem);
				context.SaveChanges();
			}

			using (var context = new CatalogDBContext(options)) {
				ItemsController itemsController = new ItemsController(context, loggerStub.Object);
				var result = await itemsController.GetItemAsync(expectedItem.Id);


				result.Value.Should().BeEquivalentTo(expectedItem);
			}

			await Task.CompletedTask;
		}

		[Fact]
		public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems() {
			var options = new DbContextOptionsBuilder<CatalogDBContext>().UseInMemoryDatabase(databaseName: "GetItemsAsync_WithExistingItems_ReturnsAllItems").Options;

			var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

			using (var context = new CatalogDBContext(options)) {
				context.Items.AddRange(expectedItems);
				context.SaveChanges();
			}

			using (var context = new CatalogDBContext(options)) {
				ItemsController itemsController = new ItemsController(context, loggerStub.Object);
				var result = await itemsController.GetItemsAsync();


				result.Should().BeEquivalentTo(expectedItems);
			}
		}

		[Fact]
		public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem() {
			var options = new DbContextOptionsBuilder<CatalogDBContext>().UseInMemoryDatabase(databaseName: "CreateItemAsync_WithItemToCreate_ReturnsCreatedItem").Options;

			var validityStart = DateTime.Now;
			var validityEnd = DateTime.Now.AddYears(rand.Next(10));

			var itemToCreate = new CreateItemDto("Objet 1", validityStart, validityEnd);

			using (var context = new CatalogDBContext(options)) {
				ItemsController itemsController = new ItemsController(context, loggerStub.Object);
				var result = await itemsController.CreateItemAsync(itemToCreate);


				if (result.Value != null) {
					var createdItem = result.Value as ItemDto;
					itemToCreate.Should().BeEquivalentTo(createdItem, options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());
					createdItem.ValidityStart.Should().BeSameDateAs(validityStart);
					createdItem.ValidityEnd.Should().BeSameDateAs(validityEnd);
				} else {
					Assert.False(true);
				}
			}
		}

		[Fact]
		public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent() {
			var options = new DbContextOptionsBuilder<CatalogDBContext>().UseInMemoryDatabase(databaseName: "DeleteItemAsync_WithExistingItem_ReturnsNoContent").Options;

			Item existingItem = CreateRandomItem();

			using (var context = new CatalogDBContext(options)) {
				context.Items.Add(existingItem);
				context.SaveChanges();
			}

			using (var context = new CatalogDBContext(options)) {
				ItemsController itemsController = new ItemsController(context, loggerStub.Object);
				var result = await itemsController.DeleteItemAsync(existingItem.Id);


				result.Should().BeOfType<NoContentResult>();

			}
		}

		private Item CreateRandomItem() {
			return new() {
				Id = Guid.NewGuid().GetHashCode(),
				Name = Guid.NewGuid().ToString(),
				ValidityStart = DateTime.Now,
				ValidityEnd = DateTime.Now.AddYears(rand.Next(10))
			};
		}
	}
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos {
	public record ItemDto(int Id, string Name, DateTime ValidityStart, DateTime ValidityEnd);
	public record CreateItemDto([Required] string Name, DateTime ValidityStart, DateTime ValidityEnd);
	public record UpdateItemDto([Required] string Name, DateTime ValidityStart, DateTime ValidityEnd);
}
using System;
using System.Collections.Generic;

#nullable disable

namespace Catalog.Api.Models {
	public partial class Item {
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime ValidityStart { get; set; }
		public DateTime ValidityEnd { get; set; }
	}
}

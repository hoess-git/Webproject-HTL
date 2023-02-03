using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Dtos
{
    public class ItemDto : ItemCreateEditDto
    {
		public List<OrderDto> Orders { get; set; }
	}

    public class ItemCreateEditDto
    {
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }
		[Required]
		public int Price { get; set; }
	}
}

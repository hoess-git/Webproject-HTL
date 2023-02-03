using DATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Dtos
{
    public class OrderDto : OrderCreateEditDto
	{
		public Customer Customer { get; set; }
	}

	public class OrderCreateEditDto
	{

		public int Id { get; set; }
		[Required]
		public DateTime Date { get; set; }
		[Required]
		public int CustomerId { get; set; }
		public List<ItemDto> Items { get; set; }
	}
}

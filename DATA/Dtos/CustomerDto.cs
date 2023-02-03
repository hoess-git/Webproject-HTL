using DATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Dtos
{
    public class CustomerDto : CustomerCreateEditDto
    {
		public List<OrderDto> Orders { get; set; }
	}

	public class CustomerCreateEditDto
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Address { get; set; }
	}
}

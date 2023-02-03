using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Entities
{
	public class ItemOrder
	{
		public int Id { get; set; }
		[Required]
		public int ItemId { get; set; }
		[Required] 
		public int OrderId { get; set; }
	}
}

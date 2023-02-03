using AutoMapper;
using DATA.Dtos;
using DATA.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CORE.Mapper
{
	public class MapConfig : Profile
	{
		public MapConfig()
		{
			CreateMap<Customer, CustomerDto>()
				.BeforeMap((src, dst) =>
				{
					if (src.Orders != null)
						foreach (var order in src.Orders)
							order.Customer = new();
				});
			CreateMap<CustomerDto, Customer>()
				.ForMember(src => src.Orders, opt => opt.Ignore());
			CreateMap<Customer, CustomerCreateEditDto>().ReverseMap();
			CreateMap<CustomerDto, CustomerCreateEditDto>().ReverseMap();

			CreateMap<Item, ItemDto>().ReverseMap();
			CreateMap<Item, ItemCreateEditDto>().ReverseMap();
			CreateMap<ItemDto, ItemCreateEditDto>().ReverseMap();

			CreateMap<Order, OrderDto>()
				.BeforeMap((src, dst) => {
					if (src.Customer != null)
						src.Customer.Orders = new List<Order>();
				});
			CreateMap<OrderDto, Order>()
				.ForMember(src => src.Customer, opt => opt.Ignore());
			CreateMap<Order, OrderCreateEditDto>().ReverseMap();
			CreateMap<OrderDto, OrderCreateEditDto>().ReverseMap();
		}
	}
}

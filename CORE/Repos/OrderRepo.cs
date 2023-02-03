using AutoMapper;
using DATA.Dtos;
using DATA.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Repos
{
	public class OrderRepo : IOrderRepo
	{
		private readonly DATA.ApiDbContext Db;
		private readonly IMapper mapper;

		public OrderRepo(DATA.ApiDbContext db, IMapper mapper)
		{
			this.Db = db;
			this.mapper = mapper;
		}

		public async Task CreateAsync(OrderCreateEditDto objDTO)
		{
			var obj = mapper.Map<OrderCreateEditDto, Order>(objDTO);
			Db.Orders.Add(obj);
			await Db.SaveChangesAsync();
		}


		public async Task<OrderDto> GetAsync(int id)
		{
			var obj = await Db.Orders.FirstOrDefaultAsync(c => c.Id == id);
			if (obj != null)
			{
				var itemOrders = await Db.ItemOrders.Where(io => io.OrderId == obj.Id).ToListAsync();
				List<Item> items = new List<Item>();
				foreach (var item in itemOrders)
				{
					var i = await Db.Items.Where(i => i.Id == item.ItemId).SingleAsync();
					items.Add(i);
				}
				obj.Items = items;
				obj.Customer = await Db.Customers.Where(c => c.Id == obj.CustomerId).SingleAsync();
			}
			return mapper.Map<Order, OrderDto>(obj);
		}

		public async Task<List<OrderDto>> GetAllAsync()
		{
			var obj = await Db.Orders.ToListAsync();
			if (obj != null)
			{
				foreach (var order in obj)
				{
					var itemOrders = await Db.ItemOrders.Where(io => io.OrderId == order.Id).ToListAsync();
					List<Item> items = new List<Item>();
					foreach (var item in itemOrders)
					{
						var i = await Db.Items.Where(i => i.Id == item.ItemId).SingleAsync();
						items.Add(i);
					}
					order.Items = items;
					order.Customer = await Db.Customers.Where(c => c.Id == order.CustomerId).SingleAsync();
				}
			}
			return mapper.Map<List<Order>, List<OrderDto>>(obj);
		}

		public async Task<int> RemoveAsync(int id)
		{
			var obj = await Db.Orders.FirstOrDefaultAsync(u => u.Id == id);
			if (obj != null)
			{
				Db.Orders.Remove(obj);
				return await Db.SaveChangesAsync();
			}
			return 0;
		}

		public async Task UpdateAsync(OrderCreateEditDto objDTO)
		{
			var obj = mapper.Map<OrderCreateEditDto, Order>(objDTO);
			Db.Orders.Update(obj);
		}
	}
}

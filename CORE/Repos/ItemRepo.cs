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
	public class ItemRepo : IItemRepo
	{
		private readonly DATA.ApiDbContext Db;
		private readonly IMapper mapper;

		public ItemRepo(DATA.ApiDbContext db, IMapper mapper)
		{
			this.Db = db;
			this.mapper = mapper;
		}

		public async Task CreateAsync(ItemCreateEditDto objDTO)
		{
			var obj = mapper.Map<ItemCreateEditDto, Item>(objDTO);
			Db.Items.Add(obj);
			await Db.SaveChangesAsync();
		}


		public async Task<ItemDto> GetAsync(int id)
		{
			var obj = await Db.Items.FirstOrDefaultAsync(c => c.Id == id);
			if (obj != null)
				obj.Orders = Db.Orders.Where(o => o.Items.Contains(obj)).ToList();
			return mapper.Map<Item, ItemDto>(obj);
		}

		public async Task<List<ItemDto>> GetAllAsync()
		{
			var obj = await Db.Items.ToListAsync(); 
			foreach (var item in obj)
				item.Orders = await Db.Orders.Where(o => o.Items.Contains(item)).ToListAsync();
			return mapper.Map<List<Item>, List<ItemDto>>(obj);
		}

		public async Task<int> RemoveAsync(int id)
		{
			var obj = await Db.Items.FirstOrDefaultAsync(u => u.Id == id);
			if (obj != null)
			{
				Db.Items.Remove(obj);
				var orders = Db.Orders.Where(o => o.Items.Contains(obj)).ToList();
				foreach (var o in orders)
					Db.Orders.Remove(o);
				return await Db.SaveChangesAsync();
			}
			return 0;
		}

		public async Task UpdateAsync(ItemCreateEditDto objDTO)
		{
			var obj = mapper.Map<ItemCreateEditDto, Item>(objDTO);
			Db.Items.Update(obj);
		}
	}
}

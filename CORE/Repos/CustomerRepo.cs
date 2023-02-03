using AutoMapper;
using DATA;
using DATA.Dtos;
using DATA.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace CORE.Repos
{
	public class CustomerRepo : ICustomerRepo
	{
		private readonly DATA.ApiDbContext Db;
		private readonly IMapper mapper;

		public CustomerRepo(DATA.ApiDbContext db, IMapper mapper)
		{
			this.Db = db;
			this.mapper = mapper;
		}

		public async Task CreateAsync(CustomerCreateEditDto objDTO)
		{
			var obj = mapper.Map<CustomerCreateEditDto, Customer>(objDTO);
			Db.Customers.Add(obj);
			await Db.SaveChangesAsync();
		}


		public async Task<CustomerDto> GetAsync(int id)
		{
			var obj = await Db.Customers.FirstOrDefaultAsync(c => c.Id == id);
			if (obj != null)
				obj.Orders = await Db.Orders.Where(o => o.CustomerId == id).ToListAsync();
			return mapper.Map<Customer, CustomerDto>(obj);
		}

		public async Task<List<CustomerDto>> GetAllAsync()
		{
			var obj = await Db.Customers.ToListAsync();
			foreach(var customer in obj)
				customer.Orders = await Db.Orders.Where(o => o.CustomerId == customer.Id).ToListAsync();
			return mapper.Map<List<Customer>, List<CustomerDto>>(obj);
		}

		public async Task<int> RemoveAsync(int id)
		{
			var obj = await Db.Customers.FirstOrDefaultAsync(u => u.Id == id);
			if (obj != null)
			{
				Db.Customers.Remove(obj);
				var orders = Db.Orders.Where(o => o.CustomerId == id).ToList();
				foreach (var o in orders)
					Db.Orders.Remove(o);
				return await Db.SaveChangesAsync();
			}
			return 0;
		}

		public async Task UpdateAsync(CustomerCreateEditDto objDTO)
		{
			var obj = mapper.Map<CustomerCreateEditDto, Customer>(objDTO);
			Db.Customers.Update(obj);
		}
	}
}

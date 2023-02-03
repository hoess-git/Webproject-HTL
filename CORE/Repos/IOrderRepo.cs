using AutoMapper;
using DATA;
using DATA.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace CORE.Repos
{
	public interface IOrderRepo
	{
		Task<List<OrderDto>> GetAllAsync();
		Task<OrderDto> GetAsync(int id);
		Task CreateAsync(OrderCreateEditDto order);
		Task UpdateAsync(OrderCreateEditDto order);
		Task<int> RemoveAsync(int id);
	}
}

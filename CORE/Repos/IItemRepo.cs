using DATA.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Repos
{
	public interface IItemRepo
	{
		Task<List<ItemDto>> GetAllAsync();
		Task<ItemDto> GetAsync(int id);
		Task CreateAsync(ItemCreateEditDto item);
		Task UpdateAsync(ItemCreateEditDto item);
		Task<int> RemoveAsync(int id);
	}
}

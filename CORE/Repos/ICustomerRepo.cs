using DATA.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Repos
{
	public interface ICustomerRepo
	{
		Task<List<CustomerDto>> GetAllAsync();
		Task<CustomerDto> GetAsync(int id);
		Task CreateAsync(CustomerCreateEditDto customer);
		Task UpdateAsync(CustomerCreateEditDto customer);
		Task<int> RemoveAsync(int id);
	}
}

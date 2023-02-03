using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using System.Net;
using DATA.Dtos;
using CORE.Repos;
using DATA.Entities;

namespace API.Endpoints
{
	public static class CustomerEndpoint
	{
		public static void ConfigureCustomerEndpoint(this WebApplication app)
		{
			app.MapGet("api/customer", GetAllCustomer)
				.WithName("GetCustomers")
				.Produces<List<CustomerDto>>(200);

			app.MapGet("api/customer/{id:int}", GetCustomer)
				.WithName("GetCustomer")
				.Produces<CustomerDto>(200);

			app.MapPost("/api/createcustomer", CreateCustomer)
				.WithName("CreateCustomer")
				.Accepts<CustomerCreateEditDto>("application/json")
				.Produces<CustomerDto>(201)
				.Produces(400);

			app.MapPut("/api/updatecustomer", UpdateCustomer)
				.WithName("UpdateCustomer")
				.Accepts<CustomerCreateEditDto>("application/json")
				.Produces<CustomerDto>(200)
				.Produces(400);

			app.MapDelete("api/deletecustomer/{id:int}", DeleteCustomer);
		}

		private async static Task<IResult> GetAllCustomer(ICustomerRepo customerRepo, ILogger<Program> _logger)
		{
			_logger.Log(LogLevel.Information, "Getting all Customers");
			IEnumerable<CustomerDto> response = await customerRepo.GetAllAsync();
			return Results.Ok(response);
		}

		private async static Task<IResult> GetCustomer(ICustomerRepo customerRepo, ILogger<Program> _logger, int id)
		{
			_logger.Log(LogLevel.Information, "Getting Customer");
			CustomerDto response = await customerRepo.GetAsync(id);
			return Results.Ok(response);
		}

		private async static Task<IResult> CreateCustomer(ICustomerRepo customerRepo, ILogger<Program> _logger, [FromBody] CustomerCreateEditDto customerCEDto, IMapper mapper)
		{
			CustomerDto response = new();

			if (customerRepo.GetAsync(customerCEDto.Id).GetAwaiter().GetResult() != null)
				return Results.BadRequest(response);

			await customerRepo.CreateAsync(customerCEDto);

			response = mapper.Map<CustomerDto>(customerCEDto);
			return Results.Ok(response);
		}

		private async static Task<IResult> UpdateCustomer(ICustomerRepo customerRepo, IMapper mapper, [FromBody] CustomerCreateEditDto customerCEDto)
		{
			await customerRepo.UpdateAsync(customerCEDto);

			CustomerDto response = mapper.Map<CustomerDto>(await customerRepo.GetAsync(customerCEDto.Id));
			return Results.Ok(response);
		}

		private async static Task<IResult> DeleteCustomer(ICustomerRepo customerRepo, int id)
		{
			CustomerDto response = new();
			try
			{
				await customerRepo.RemoveAsync(id);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				return Results.BadRequest(response);
			}
		}
	}
}

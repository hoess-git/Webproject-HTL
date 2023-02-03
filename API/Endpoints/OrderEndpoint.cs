using AutoMapper;
using CORE.Repos;
using DATA.Dtos;
using DATA.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Endpoints
{
	public static class OrderEndpoint
	{
		public static void ConfigureOrderEndpoint(this WebApplication app)
		{
			app.MapGet("api/order", GetAllOrder)
				.WithName("GetOrders")
				.Produces<List<OrderDto>>(200);

			app.MapGet("api/order/{id:int}", GetOrder)
				.WithName("GetOrder")
				.Produces<OrderDto>(200);

			app.MapPost("/api/createorder", CreateOrder)
				.WithName("CreateOrder")
				.Accepts<OrderCreateEditDto>("application/json")
				.Produces<OrderDto>(201)
				.Produces(400);

			app.MapPut("/api/updateorder", UpdateOrder)
				.WithName("UpdateOrder")
				.Accepts<OrderCreateEditDto>("application/json")
				.Produces<OrderDto>(200)
				.Produces(400);

			app.MapDelete("api/deleteorder/{id:int}", DeleteOrder);
		}

		private async static Task<IResult> GetAllOrder(IOrderRepo orderRepo, ILogger<Program> _logger)
		{
			_logger.Log(LogLevel.Information, "Getting all Items");
			IEnumerable<OrderDto> response = await orderRepo.GetAllAsync();
			return Results.Ok(response);
		}

		private async static Task<IResult> GetOrder(IOrderRepo orderRepo, ILogger<Program> _logger, int id)
		{
			_logger.Log(LogLevel.Information, "Getting Item");
			OrderDto response = await orderRepo.GetAsync(id);
			return Results.Ok(response);
		}

		private async static Task<IResult> CreateOrder(IOrderRepo orderRepo, ILogger<Program> _logger, [FromBody] OrderCreateEditDto orderCEDto, IMapper mapper)
		{
			OrderDto response = new();

			if (orderRepo.GetAsync(orderCEDto.Id).GetAwaiter().GetResult() != null)
				return Results.BadRequest(response);

			await orderRepo.CreateAsync(orderCEDto);

			response = mapper.Map<OrderDto>(orderCEDto);
			return Results.Ok(response);
		}

		private async static Task<IResult> UpdateOrder(IOrderRepo orderRepo, IMapper mapper, [FromBody] OrderCreateEditDto orderCEDto)
		{
			await orderRepo.UpdateAsync(orderCEDto);

			OrderDto response = mapper.Map<OrderDto>(await orderRepo.GetAsync(orderCEDto.Id));
			return Results.Ok(response);
		}

		private async static Task<IResult> DeleteOrder(IOrderRepo orderRepo, int id)
		{
			OrderDto response = new();
			try
			{
				await orderRepo.RemoveAsync(id);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				return Results.BadRequest(response);
			}
		}
	}
}

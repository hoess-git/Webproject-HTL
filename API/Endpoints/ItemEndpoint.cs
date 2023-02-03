using AutoMapper;
using CORE.Repos;
using DATA.Dtos;
using DATA.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Endpoints
{
	public static class ItemEndpoint
	{
		public static void ConfigureItemEndpoint(this WebApplication app)
		{
			app.MapGet("api/item", GetAllItem)
				.WithName("GetItems")
				.Produces<List<ItemDto>>(200);

			app.MapGet("api/item/{id:int}", GetItem)
				.WithName("GetItem")
				.Produces<ItemDto>(200);

			app.MapPost("/api/createitem", CreateItem)
				.WithName("CreateItem")
				.Accepts<ItemCreateEditDto>("application/json")
				.Produces<ItemDto>(201)
				.Produces(400);

			app.MapPut("/api/updateitem", UpdateItem)
				.WithName("UpdateItem")
				.Accepts<ItemCreateEditDto>("application/json")
				.Produces<ItemDto>(200)
				.Produces(400);

			app.MapDelete("api/deleteitem/{id:int}", DeleteItem);
		}

		private async static Task<IResult> GetAllItem(IItemRepo itemRepo, ILogger<Program> _logger)
		{
			_logger.Log(LogLevel.Information, "Getting all Items");
			List<ItemDto> response = await itemRepo.GetAllAsync();
			return Results.Ok(response);
		}

		private async static Task<IResult> GetItem(IItemRepo itemRepo, ILogger<Program> _logger, int id)
		{
			_logger.Log(LogLevel.Information, "Getting Item");
			ItemDto response = await itemRepo.GetAsync(id);
			return Results.Ok(response);
		}

		private async static Task<IResult> CreateItem(IItemRepo itemRepo, ILogger<Program> _logger, [FromBody] ItemCreateEditDto itemCEDto, IMapper mapper)
		{
			ItemDto response = new();

			if (itemRepo.GetAsync(itemCEDto.Id).GetAwaiter().GetResult() != null)
				return Results.BadRequest(response);

			await itemRepo.CreateAsync(itemCEDto);

			response = mapper.Map<ItemDto>(itemCEDto);
			return Results.Ok(response);
		}

		private async static Task<IResult> UpdateItem(IItemRepo itemRepo, IMapper mapper, [FromBody] ItemCreateEditDto itemCEDto)
		{
			await itemRepo.UpdateAsync(itemCEDto);

			ItemDto response = mapper.Map<ItemDto>(await itemRepo.GetAsync(itemCEDto.Id));
			return Results.Ok(response);
		}

		private async static Task<IResult> DeleteItem(IItemRepo itemRepo, int id)
		{
			ItemDto response = new();
			try
			{
				await itemRepo.RemoveAsync(id);
				return Results.Ok(response);
			}
			catch (Exception ex)
			{
				return Results.BadRequest(response);
			}
		}
	}
}

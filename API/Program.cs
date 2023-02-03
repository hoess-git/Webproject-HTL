using API.Endpoints;
using CORE.Mapper;
using CORE.Repos;
using DATA;
using DATA.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Models;

var cors = "_cors";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option => {
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description =
			 "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
			 "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
			 "Example: \"Bearer 12345abcdef\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header,

						},
						new List<string>()
					}
				});
});

builder.Services.AddAutoMapper(typeof(MapConfig));
builder.Services.AddDbContext<ApiDbContext>(option =>
	option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IItemRepo, ItemRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true));

app.UseAuthentication();
//app.UseAuthorization();

app.ConfigureCustomerEndpoint();
app.ConfigureItemEndpoint();
app.ConfigureOrderEndpoint();

app.UseHttpsRedirection();

app.Run();
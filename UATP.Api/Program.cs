using Microsoft.EntityFrameworkCore;
using UATP.Core.Interfaces;
using UATP.Core.Services;
using UATP.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
              throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connStr));

builder.Services.AddControllers();

builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
builder.Services.AddScoped<IPaymentTransactionService, PaymentTransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var useHttps = builder.Configuration.GetValue<bool>("UseHttps");

if(useHttps)
    app.UseHttpsRedirection();

app.MapControllers();

app.Run();

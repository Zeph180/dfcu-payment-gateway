using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.Services;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Data;
using PaymentGateway.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); // 👈 this is the missing line

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<PaymentService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); // needed to handle controller routing

app.Run();
using System.Text;
using EasyNetQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Repository.DbContext;
using Portfolio.Repository.Interfaces;
using Portfolio.Repository.Repositories;
using Portfolio.Services.Interfaces;
using Portfolio.Services.Services;
using Shared.Messages;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PortfolioContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostGresConnectionString")));

builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
var bus = RabbitHutch.CreateBus("host=rabbitmq");
builder.Services.AddSingleton(bus);

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });
var app = builder.Build();
DbMgmt.MigrationInit(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

var lifetimeScope = app.Services.CreateScope().ServiceProvider;
var portfolioService = lifetimeScope.GetRequiredService<IPortfolioService>();

bus.PubSub.Subscribe<CreatePortfolioMessage>("portfoliocreate", async message =>
{
    Console.WriteLine($"Recieved Message: {message}");
    await portfolioService.CreatePortfolioAsync(message.userId);
});

bus.PubSub.Subscribe<AddInvestmentMessage>("investmentadd", async message =>
{
    Console.WriteLine($"Recieved Message: {message}");
    await portfolioService.AddInvestmentAsync(message.userId, message.projectId, message.amount);
});

app.Run();
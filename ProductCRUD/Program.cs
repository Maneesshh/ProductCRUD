using Microsoft.EntityFrameworkCore;
using ProductCRUD.Data;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<DbContext, AppDbContext>();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Register the MySQL-specific DbContext with the correct connection string
string connectionString = builder.Configuration.GetConnectionString("MyServer");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
               builder =>
               {
                   builder
                       .AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials(); ;
               });
});

builder.Services.AddSwaggerGen();

// Remove MySQL-specific switch if not needed
// AppContext.SetSwitch("Mysql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoApiConnection")));
builder.Services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{id?}");

app.Run();

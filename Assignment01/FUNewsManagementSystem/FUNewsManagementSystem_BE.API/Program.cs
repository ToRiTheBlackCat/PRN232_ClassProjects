using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<NewsArticleService>();

builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<NewsArticleRepository>();
builder.Services.AddScoped<CategoryRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

using FUNewsManagementSystem.Service;
using System.Text.Json;
using System.Text.Json.Serialization;
using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddScoped<NewsArticleService>();
builder.Services.AddScoped<NewsArticleRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

    builder.Services.AddDbContext<FUNewsManagementContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });

    });
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
    app.UseCors("AllowAll");

    app.UseHttpsRedirection();

    //app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
});

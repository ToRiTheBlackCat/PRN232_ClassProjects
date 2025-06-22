using FUNewsManagementSystem.Service;
using System.Text.Json;
using System.Text.Json.Serialization;
using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<NewsArticleModel>("NewsArticles");

    builder.EntitySet<NewsArticleView>("NewsArticlesViews")
        .EntityType.HasKey(n => n.NewsArticleId);

    builder.EntitySet<SystemAccountView>("SystemAccountsViews")
        .EntityType.HasKey(a => a.AccountId);

    builder.EntitySet<TagView>("TagsViews")
        .EntityType.HasKey(t => t.TagId);

    var functionNewsCount = builder.Function("GetTotalNewsCountOData")
    .Returns<int>();

    functionNewsCount.Parameter<string>("categoryName");
    functionNewsCount.Parameter<DateTime?>("fromDate");
    functionNewsCount.Parameter<DateTime?>("toDate");
    //builder.EntitySet<T>();
    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers();

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FUNewsManagementSystem API",
        Version = "v1"
    });

    c.EnableAnnotations(); 
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .AddOData(options => options
    .AddRouteComponents("odata", GetEdmModel())
    .Select()
    .Filter()
    .OrderBy()
    .SetMaxTop(20)
    .Count()
    .Expand()
);


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

app.UseRouting();

app.MapControllers();

app.Run();

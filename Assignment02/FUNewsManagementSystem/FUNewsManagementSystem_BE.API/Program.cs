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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

builder.Services.AddDbContext<FUNewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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

builder.Services.AddScoped<NewsArticleService>();
builder.Services.AddScoped<NewsArticleRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<NewsArticleService>();

builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<NewsArticleRepository>();
builder.Services.AddScoped<CategoryRepository>();

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
        };
    });


builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "JWT Authentication for Cosmetics Management",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowAll");


app.UseHttpsRedirection();

app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();

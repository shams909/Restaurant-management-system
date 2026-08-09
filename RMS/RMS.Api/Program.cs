using RMS.Api.Middleware;
using RMS.Api.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RMS.Application.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using RMS.Application.Interfaces;
using RMS.Infrastructure.Persistence;
using RMS.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
// 1. Load the secret .env file
Env.Load();

// 2. Grab the password from the environment
var connectionString = Environment.GetEnvironmentVariable("RMS_DB_CONNECTION");




// 3. Securely connect to the database!
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


var secretKey = Environment.GetEnvironmentVariable("JWT_KEY");
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// [NEW] Configure CORS so the React frontend can talk to the API!
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // 1. Adds the "Authorize" button to the top of Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // 2. Tells Swagger to automatically attach the Token to every locked endpoint
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new System.Collections.Generic.List<string>()
        }
    });
});


// This enables the IHttpContextAccessor to read HTTP requests!
builder.Services.AddHttpContextAccessor();

// This injects the CurrentUserService into the pipeline!
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


// 1. This connects your C# Bridge to the Professor's remote database
///(Now, the API will completely ignore passwords and trust your ApplicationDbContext to handle its own connection!)


// 2. This injects the Unit of Work engine into every single API Controller
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 3. This injects the TenantService (The Chef) into the Controller
builder.Services.AddScoped<ITenantService, TenantService>();

// 4. This injects the BranchService (The Chef) into the Controller
builder.Services.AddScoped<IBranchService, BranchService>();

// This injects the UserService (The Chef) into the Controller
builder.Services.AddScoped<IUserService, UserService>();

// This injects the CustomerService (The Chef) into the Controller
builder.Services.AddScoped<IMenuCategoryService, MenuCategoryService>();

// This injects the MenuItemService (The Chef) into the Controller
builder.Services.AddScoped<IMenuItemService, MenuItemService>();

// This injects the ItemVariantService (The Chef) into the Controller
builder.Services.AddScoped<ITableService, TableService>();

// This injects the InventoryItemService (The Chef) into the Controller
builder.Services.AddScoped<IOrderService, OrderService>();

// This injects the OrderItemService (The Chef) into the Controller
builder.Services.AddScoped<IOrderItemService, OrderItemService>();

// This injects the PaymentService (The Chef) into the Controller
builder.Services.AddScoped<IPaymentService, PaymentService>();

// This injects the InventoryItemService (The Chef) into the Controller
builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();


builder.Services.AddScoped<ICustomerService, CustomerService>();
// This injects the RecipeService (The Chef) into the Controller
builder.Services.AddScoped<IRecipeService, RecipeService>();

// This injects the AuthService (The Chef) into the Controller
builder.Services.AddScoped<IAuthService, AuthService>();

// This injects the KitchenService (The Chef) into the Controller
builder.Services.AddScoped<IKitchenService, KitchenService>();

// This injects the ReportingService (The Chef) into the Controller
builder.Services.AddScoped<IReportingService, ReportingService>();


// This scans your entire project, finds the MappingProfile, and turns on AutoMapper!
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<RMS.Application.Mappings.MappingProfile>();
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// [NEW] 1. Catch all crashes and turn them into JSON!
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

// [NEW] 2. Open the gates for the React Frontend!
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


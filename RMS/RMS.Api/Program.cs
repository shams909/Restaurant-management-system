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


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IRecipeService, RecipeService>();


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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

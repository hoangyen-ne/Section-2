using coffeeshop.Data;
using coffeeshop.Models.Interfaces;
using coffeeshop.Models.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

//Add code
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository>(ShoppingCartRepository.GetCart);
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddDbContext<CoffeeShopDbContext>(option =>option.UseSqlServer(builder.Configuration.GetConnectionString("CoffeeShopDbContextConnection")));
builder.Services.AddControllersWithViews();


//Add code
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

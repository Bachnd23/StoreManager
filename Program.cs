using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using COCOApp.Models;
using Newtonsoft.Json;
using COCOApp.Services;
using Microsoft.Extensions.Configuration;
using COCOApp.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string from configuration
string connectionStr = builder.Configuration.GetConnectionString("MyConStr");

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddDbContext<StoreManagerContext>(opt =>
    opt.UseSqlServer(connectionStr));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// Correctly get the email settings from the configuration
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddSingleton(emailSettings);
builder.Services.AddTransient<EmailService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportsOrdersMappingRepository, ReportsOrdersMappingRepository>();
builder.Services.AddScoped<ISellerDetailRepository, SellerDetailRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Register your custom services here
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<ReportsOrdersMappingService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SellerDetailsService>();
// Configure SignalR to handle cyclic references
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.PayloadSerializerOptions.WriteIndented = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=ViewSignIn}");
    endpoints.MapHub<ProductHub>("/productHub"); 
    endpoints.MapHub<CustomerHub>("/customerHub"); 
});
app.Run();

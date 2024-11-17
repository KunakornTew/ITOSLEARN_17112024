using ITOS_LEARN.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Session
builder.Services.AddDistributedMemoryCache();  // �� memory cache ����Ѻ session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ��˹����� session �������
    options.Cookie.HttpOnly = true; // ��� session cookie �������ö��Ҷ֧��ҡ JavaScript
    options.Cookie.IsEssential = true; // �к���� session cookie ����觷�����
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

// Use routing
app.UseRouting();

// Use session
app.UseSession(); // ��ͧ�� session ��ѧ�ҡ `app.UseRouting()` ��С�͹ `app.UseAuthorization()`

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
await app.RunAsync();

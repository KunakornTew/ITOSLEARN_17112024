using ITOS_LEARN.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Session
builder.Services.AddDistributedMemoryCache();  // ใช้ memory cache สำหรับ session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // กำหนดเวลา session หมดอายุ
    options.Cookie.HttpOnly = true; // ให้ session cookie ไม่สามารถเข้าถึงได้จาก JavaScript
    options.Cookie.IsEssential = true; // ระบุว่า session cookie เป็นสิ่งที่จำเป็น
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
app.UseSession(); // ต้องใช้ session หลังจาก `app.UseRouting()` และก่อน `app.UseAuthorization()`

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
await app.RunAsync();

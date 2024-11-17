using Microsoft.AspNetCore.Mvc;
using ITOS_LEARN.Models;
using ITOS_LEARN.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace ITOS_LEARN.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> passwordHasher;

        public AccountController(ApplicationDbContext context)
        {
            passwordHasher = new PasswordHasher<User>();
            _context = context;
        }

        // หน้า Login GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // หน้า Login POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Email == model.Email);

                var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (user != null)
                {
                    if (result == PasswordVerificationResult.Success)
                    {
                        // เก็บข้อมูล username และ role ลงใน session
                        HttpContext.Session.SetString("User", user.Username);
                        HttpContext.Session.SetString("Role", user.Role); // เก็บบทบาทใน session

                        var login = new Login
                        {
                            Username = user.Username,
                            LoginTime = DateTime.Now,
                            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() // เก็บ IP Address
                        };

                        _context.Logins.Add(login); // เพิ่ม Login ลงในฐานข้อมูล
                        await _context.SaveChangesAsync(); // บันทึกการเปลี่ยนแปลง

                        // ตั้งค่า session เมื่อผู้ใช้ล็อกอินสำเร็จ
                        HttpContext.Session.SetString("User", user.Username); // set session

                        // พาผู้ใช้ไปหน้า People (หน้า Details)
                        return RedirectToAction("Index", "People");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username, password, or email.");
                }
            }
            return View(model);
        }


        // หน้า Register GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // หน้า Register POST
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var existingEmail = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                var hashPassword = passwordHasher.HashPassword(user, user.Password);
                user.Password = hashPassword;

                // บันทึกข้อมูลผู้ใช้ใหม่
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // เก็บชื่อผู้ใช้จาก session ก่อน
            var username = HttpContext.Session.GetString("User");

            if (!string.IsNullOrEmpty(username))
            {
                // เพิ่ม LogoutTime ในฐานข้อมูล
                var login = await _context.Logins
                    .Where(l => l.Username == username && l.LogoutTime == null)  // ตรวจสอบว่า LogoutTime ยังเป็น null
                    .OrderByDescending(l => l.LoginTime)
                    .FirstOrDefaultAsync();

                if (login != null)
                {
                    login.LogoutTime = DateTime.Now;
                    await _context.SaveChangesAsync();  // บันทึกการเปลี่ยนแปลง
                }

                // ลบข้อมูลใน session ที่เก็บ user
                HttpContext.Session.Remove("User");
            }

            // พาผู้ใช้กลับไปหน้า Login
            return RedirectToAction("Login", "Account");
        }
    }
}

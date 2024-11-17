using Microsoft.AspNetCore.Mvc;
using ITOS_LEARN.Models;
using ITOS_LEARN.Data;
using Microsoft.EntityFrameworkCore;

namespace ITOS_LEARN.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index(string searchTerm)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // ส่งค่าของ searchTerm ไปยัง View
            ViewData["SearchTerm"] = searchTerm;

            List<Person> people;
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    people = await _context.People
                        .Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm)
                            || p.BirthDate.ToString().Contains(searchTerm) || p.JobPosition.Contains(searchTerm)
                            || p.ID.ToString().Contains(searchTerm)) // ค้นหาตาม ID
                        .ToListAsync();
                }
                else
                {
                    people = await _context.People.ToListAsync();
                }
            }
            else
            {
                ModelState.AddModelError("", "ข้อมูลไม่ถูกต้อง");
                // ดึงข้อมูลทั้งหมดมาแสดงหาก ModelState ไม่ผ่าน
                people = await _context.People.ToListAsync();
            }

            return View(people);
        }


        // GET: People/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // ตรวจสอบ ModelState ก่อนดำเนินการ
            if (!ModelState.IsValid)
            {
                return View(person); // หากไม่ถูกต้องให้กลับไปที่หน้าสร้างใหม่
            }

            // ตรวจสอบอีเมลไม่ให้ซ้ำ
            if (await IsEmailDuplicate(person.Email))
            {
                ModelState.AddModelError("Email", "อีเมลนี้ถูกใช้แล้ว");
                return View(person);
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
{
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
             var person = await _context.People.FindAsync(id);
               if (person == null)
            {
                return NotFound();
            }
                return View(person);
        }

    // ถ้าผ่านการตรวจสอบ ModelState แล้ว
    var personToUpdate = await _context.People.FindAsync(id);
    if (personToUpdate == null)
    {
        return NotFound();
    }

    // ทำการอัปเดตข้อมูลหรือตรวจสอบข้อมูลที่จะแก้ไขที่นี่ก่อน
    return View(personToUpdate);  // การแสดงผลหน้าต่างการแก้ไข
}


        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person person)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != person.ID)
            {
                return NotFound();
            }

            // ตรวจสอบ ModelState ก่อนดำเนินการ
            if (!ModelState.IsValid)
            {
                return View(person); // หากไม่ถูกต้องให้กลับไปที่หน้าก่อนหน้า
            }

            // ตรวจสอบอีเมลไม่ให้ซ้ำในขณะที่ทำการอัปเดต
            if (await IsEmailDuplicate(person.Email, person.ID))
            {
                ModelState.AddModelError("Email", "อีเมลนี้ถูกใช้แล้ว");
                return View(person);
            }

            _context.Update(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var person = await _context.People
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (person == null)
                {
                    return NotFound();
                }

                return View(person);
            }
            return View(null);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // ตรวจสอบ ModelState ก่อนดำเนินการ
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index)); // หากไม่ถูกต้องให้ย้ายไปที่หน้าหลัก
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ฟังก์ชันตรวจสอบอีเมลซ้ำ
        private async Task<bool> IsEmailDuplicate(string email, int? excludeId = null)
        {
            return await _context.People
                .AnyAsync(p => p.Email == email && (!excludeId.HasValue || p.ID != excludeId.Value));
        }
    }
}

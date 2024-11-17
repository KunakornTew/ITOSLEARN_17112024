using ITOS_LEARN.Data;
using Microsoft.AspNetCore.Mvc;

namespace ITOS_LEARN.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var people = _context.Users.ToList();
            return View(people);
        }
    }
}
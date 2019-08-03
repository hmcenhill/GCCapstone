using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCCapstone.Data;
using Microsoft.AspNetCore.Http;

namespace GCCapstone.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly ISession _session;

        public UsersController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _session = httpContextAccessor.HttpContext.Session;

        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.Where(x => x.UserId == _session.GetInt32("CurrentUserId")).FirstOrDefaultAsync();
            if (user.IsAdmin)
            {
                return View(await _context.Users.ToListAsync());
            }
            else
            {
                return RedirectToAction("Details", new { @id = user.UserId });
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            _session.SetInt32("UserID", user.UserId);

            ViewDataVM vm = new ViewDataVM();
            vm.CurrentUser = user;
            vm.Enrollments = _context.Enrollments.Where(x => x.User.UserId == user.UserId).ToList();
            vm.Courses = new List<Course>();
            foreach(var enrollment in vm.Enrollments)
            {
                vm.Courses.Add(_context.Courses.Where(x => x.CourseId == enrollment.CourseId).FirstOrDefault());
            }

            return View(vm);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password,FirstName,LastName,Role,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                _session.SetInt32("CurrentUserId", user.UserId);
                return RedirectToAction("Details", new { @id = user.UserId });
            }
            return View(user);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] User attempt)
        {
            if (ModelState.IsValid)
            {
                var tryLogin = await _context.Users.Where(x => x.Username == attempt.Username).FirstOrDefaultAsync();
                if(tryLogin != null && attempt.Password == tryLogin.Password)
                {
                    _session.SetInt32("CurrentUserId", tryLogin.UserId);
                    return RedirectToAction("Details", new { @id = tryLogin.UserId });
                }
            }
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Username","Password","UserId,FirstName,LastName,Role,IsAdmin")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_Sisgen.Data;
using Portal_Sisgen.Models;
using Portal_Sisgen.Utility;

namespace Portal_Sisgen.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminUser)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = SD.AdminUser)]
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return View(await _db.ApplicationUser.Where(u=>u.Id != claim.Value).ToListAsync());
        }



        //GET - EDIT
        [Authorize(Roles = SD.AdminUser)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationUser = await _db.ApplicationUser.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.AdminUser)]
        public async Task<IActionResult> Edit(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var _userFromDb = await _db.ApplicationUser.FirstOrDefaultAsync(x => x.Id == applicationUser.Id);

                _userFromDb.Nombre = applicationUser.Nombre;
                _userFromDb.PhoneNumber = applicationUser.PhoneNumber;
                _userFromDb.Email = applicationUser.Email;

                _db.Update(_userFromDb);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        //GET - DELETE
        [Authorize(Roles = SD.AdminUser)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationUser = await _db.ApplicationUser.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        //POST - DELETE

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.AdminUser)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _db.ApplicationUser.FindAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }
            _db.Remove(applicationUser);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
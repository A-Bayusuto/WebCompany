using KnifeCompany.DataAccess.Data;
using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KnifeCompany.Utility;
using System.Diagnostics;

namespace KnifeCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(string id)
        {
            ApplicationUser user = new ApplicationUser();         
            if (id == null)
            {
                // this is for create
                
                return View(user);
            }

            user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user.Id == null)
            {
                return NotFound();
            }
            Debug.WriteLine("TESTING input id: " + id);
            Debug.WriteLine("TESTING user: " + user.Id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id == null)
                {
                    _unitOfWork.ApplicationUser.Add(user);
                }
                else
                {
                    _unitOfWork.ApplicationUser.Update(user);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {   
            // fetch all the users
            var userList = _db.ApplicationUsers.Include(u => u.Company).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();


            // for each user, assigns their roles form roles table
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (user.Company == null)
                {
                    user.Company = new Company
                    {
                        Name = ""
                    };
                }
            }
            // userlist updated and can be sent by Json for JS to use for table
            return Json(new { data = userList });
        }

        [HttpPost]

        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now){
                // user is locked, we will unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation Success" });
        }



        #endregion
    }
}

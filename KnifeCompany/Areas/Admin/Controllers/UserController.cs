using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnifeCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            User user = new User();
            if (id == null)
            {
                // this is for create
                return View(user);
            }
            // this is for edit
            user = _unitOfWork.User.Get(id.GetValueOrDefault());
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id == 0)
                {
                    _unitOfWork.User.Add(user);
                }
                else
                {
                    _unitOfWork.User.Update(user);
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
            var allObj = _unitOfWork.User.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.User.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.User.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}

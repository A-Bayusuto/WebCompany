//using KnifeCompany.DataAccess.Repository.IRepository;
//using KnifeCompany.Models;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace KnifeCompany.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class UserController : Controller
//    {

//        private readonly IUnitOfWork _unitOfWork;

//        public UserController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }


//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Upsert(int? id)
//        {
//            ApplicationUser user = new ApplicationUser();
//            if (id == null)
//            {
//                // this is for create
//                return View(user);
//            }
//            // this is for edit
//            user = _unitOfWork.ApplicationUser.Get(id.GetValueOrDefault());
//            if (user == null)
//            {
//                return NotFound();
//            }

//            return View(user);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]

//        public IActionResult Upsert(ApplicationUser user)
//        {

//            return View(user);
//        }


//        #region API CALLS

//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            var allObj = _unitOfWork.ApplicationUser.GetAll();
//            return Json(new { data = allObj });
//        }



//        #endregion
//    }
//}

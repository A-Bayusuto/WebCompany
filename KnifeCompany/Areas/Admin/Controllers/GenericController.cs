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
//    public class GenericController : Controller
//    {
//        // call the 
//        private readonly IUnitOfWork _unitOfWork;

//        public GenericController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }


//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Upsert(int? id)
//        {
//            Generic generic = new Generic();
//            if (id == null)
//            {
//                // this is for create
//                return View(generic);
//            }
//            // this is for edit
//            generic = _unitOfWork.Generic.Get(id.GetValueOrDefault());
//            if (generic == null)
//            {
//                return NotFound();
//            }

//            return View(generic);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]

//        public IActionResult Upsert(Generic generic)
//        {
//            if (ModelState.IsValid)
//            {
//                if (generic.Id == 0)
//                {
//                    _unitOfWork.Generic.Add(generic);
//                }
//                else
//                {
//                    _unitOfWork.Generic.Update(generic);
//                }
//                _unitOfWork.Save();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(generic);
//        }


//        #region API CALLS

//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            var allObj = _unitOfWork.Generic.GetAll();
//            return Json(new { data = allObj });
//        }

//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            var objFromDb = _unitOfWork.Generic.Get(id);
//            if (objFromDb == null)
//            {
//                return Json(new { success = false, message = "Error while deleting" });
//            }
//            _unitOfWork.Generic.Remove(objFromDb);
//            _unitOfWork.Save();
//            return Json(new { success = true, message = "Delete Successful" });

//        }

//        #endregion
//    }
//}

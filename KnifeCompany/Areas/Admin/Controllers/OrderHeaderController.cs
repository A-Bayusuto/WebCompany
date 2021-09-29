using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using KnifeCompany.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnifeCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderHeaderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderHeaderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            OrderHeader orderHeader = new OrderHeader();
            if (id == null)
            {
                // this is for create
                return View(orderHeader);
            }
            // this is for edit
            orderHeader = _unitOfWork.OrderHeader.Get(id.GetValueOrDefault());
            if (orderHeader == null)
            {
                return NotFound();
            }

            return View(orderHeader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(OrderHeader orderHeader)
        {
            if (ModelState.IsValid)
            {
                if (orderHeader.Id == 0)
                {
                    _unitOfWork.OrderHeader.Add(orderHeader);
                }
                else
                {
                    _unitOfWork.OrderHeader.Update(orderHeader);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(orderHeader);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.OrderHeader.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.OrderHeader.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.OrderHeader.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}

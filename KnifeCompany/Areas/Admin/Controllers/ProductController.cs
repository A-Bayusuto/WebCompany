using Dapper;
using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using KnifeCompany.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnifeCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            if (id == null)
            {
                // this is for create
                return View(product);
            }
            // this is for edit
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            //retrieve object
            product = _unitOfWork.SP_Call.OneRecord<Product>(SD.Proc_Products_Get, parameter); 
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Product product)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", product.Name);
                parameter.Add("@Price", product.Price);
                parameter.Add("@Status", product.Status);
                parameter.Add("@Description", product.Description);
                parameter.Add("@Picture", product.Picture);
                if (product.Id == 0)
                {
                    _unitOfWork.SP_Call.Execute(SD.Proc_Products_Create, parameter);
                }
                else
                {
                    parameter.Add("@Id", product.Id);
                    _unitOfWork.SP_Call.Execute(SD.Proc_Products_Update, parameter);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<Product>(SD.Proc_Products_GetAll, null);
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            //retrieve object
            var objFromDb = _unitOfWork.SP_Call.OneRecord<Product>(SD.Proc_Products_Get, parameter);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.SP_Call.Execute(SD.Proc_Products_Delete, parameter);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}

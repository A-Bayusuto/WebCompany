using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using KnifeCompany.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KnifeCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;

        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };


            if (id == null)
            {
                // this is for create
                return View(productVM);
            }
            // this is for edit
            productVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                //System.Diagnostics.Debug.WriteLine("BEFORE FILE TRANSFER");
                //System.Diagnostics.Debug.WriteLine("PRODUCT: " + productVM.Product.Id);
                //System.Diagnostics.Debug.WriteLine("FILES: " + files[0].FileName);

                if (files.Count > 0)
                {
                    //System.Diagnostics.Debug.WriteLine("ENTERED HERE");
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.Picture != null)
                    {
                        // this is an edit and we need to remove old image
                        var imagePath = Path.Combine(webRootPath, productVM.Product.Picture.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    // gets picture path and copies it to the root folder for images
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    productVM.Product.Picture = @"\images\products\" + fileName + extension;
                }

                else
                {
                    // update when they do not change the image
                    if (productVM.Product.Id != 0)
                    {
                        Product objFromDb = _unitOfWork.Product.Get(productVM.Product.Id);
                        productVM.Product.Picture = objFromDb.Picture;
                    }
                }

                //System.Diagnostics.Debug.WriteLine("TESTING PIC VAL: " + productVM.Product.Picture);

                // either adds or updates product

                if (productVM.Product.Id == 0)
                {
                    Debug.WriteLine("product Name:" + productVM.Product.Name);
                    Debug.WriteLine("product Price:" + productVM.Product.Price);
                    Debug.WriteLine("product CategoryId:" + productVM.Product.CategoryId);
                    Debug.WriteLine("product Category Name:" + productVM.Product.Category.Name);
                    _unitOfWork.Product.Add(productVM.Product);
                }

                else
                {
                    Debug.WriteLine("product Name:" + productVM.Product.Name);
                    Debug.WriteLine("product Price:" + productVM.Product.Price);
                    Debug.WriteLine("product CategoryId:" + productVM.Product.CategoryId);
                    //Debug.WriteLine("product Category Name:" + productVM.Product.Category.Name);
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
                }
            }
            return View(productVM);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Product.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.Picture.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}

using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using KnifeCompany.Models.ViewModels;
using KnifeCompany.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            List<SelectListItem> orderStatusList = new List<SelectListItem>();
            orderStatusList.Add(new SelectListItem() { Text = "Pending", Value = SD.StatusPending });
            orderStatusList.Add(new SelectListItem() { Text = "Processing", Value = SD.StatusInProcess });
            orderStatusList.Add(new SelectListItem() { Text = "Completed", Value = SD.StatusCompleted });
            orderStatusList.Add(new SelectListItem() { Text = "Rejected", Value = SD.StatusCancelled });

            List<SelectListItem> paymentStatusList = new List<SelectListItem>();
            paymentStatusList.Add(new SelectListItem() { Text = "Pending", Value = SD.PaymentStatusPending });
            paymentStatusList.Add(new SelectListItem() { Text = "Approved", Value = SD.PaymentStatusApproved });
            paymentStatusList.Add(new SelectListItem() { Text = "Rejected", Value = SD.PaymentStatusRejected });

            OrderHeaderVM orderHeaderVM = new OrderHeaderVM()
            {
                OrderHeader = new OrderHeader(),
                OrderStatusList = orderStatusList,
                PaymentStatusList = paymentStatusList
            };

            if (id == null)
            {
                // this is for create
                return View(orderHeaderVM);
            }
            // this is for edit
            orderHeaderVM.OrderHeader = _unitOfWork.OrderHeader.Get(id.GetValueOrDefault());
            if (orderHeaderVM == null)
            {
                return NotFound();
            }

            return View(orderHeaderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(OrderHeaderVM orderHeaderVM)
        {
            Debug.WriteLine("Order Id: " + orderHeaderVM.OrderHeader.Id);
            Debug.WriteLine("Order AppId: " + orderHeaderVM.OrderHeader.ApplicationId);
            Debug.WriteLine("Order OrderDate: " + orderHeaderVM.OrderHeader.OrderDate);
            Debug.WriteLine("Order OrderTotal: " + orderHeaderVM.OrderHeader.OrderTotal);
            Debug.WriteLine("Order OrderStatus: " + orderHeaderVM.OrderHeader.OrderStatus);
            Debug.WriteLine("Order PaymentStatus: " + orderHeaderVM.OrderHeader.PaymentStatus);
            if (ModelState.IsValid)
            {
                if (orderHeaderVM.OrderHeader.Id == 0)
                {
                    _unitOfWork.OrderHeader.Add(orderHeaderVM.OrderHeader);
                }
                else
                {
                    _unitOfWork.OrderHeader.Update(orderHeaderVM.OrderHeader);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index", "OrderHeader");
            }

            List<SelectListItem> orderStatusList = new List<SelectListItem>();
            orderStatusList.Add(new SelectListItem() { Text = "Pending", Value = SD.StatusPending });
            orderStatusList.Add(new SelectListItem() { Text = "Processing", Value = SD.StatusInProcess });
            orderStatusList.Add(new SelectListItem() { Text = "Completed", Value = SD.StatusCompleted });
            orderStatusList.Add(new SelectListItem() { Text = "Rejected", Value = SD.StatusCancelled });

            List<SelectListItem> paymentStatusList = new List<SelectListItem>();
            paymentStatusList.Add(new SelectListItem() { Text = "Pending", Value = SD.PaymentStatusPending });
            paymentStatusList.Add(new SelectListItem() { Text = "Approved", Value = SD.PaymentStatusApproved });
            paymentStatusList.Add(new SelectListItem() { Text = "Rejected", Value = SD.PaymentStatusRejected });

            OrderHeaderVM orderHeaderVM2 = new OrderHeaderVM()
            {
                OrderHeader = orderHeaderVM.OrderHeader,
                OrderStatusList = orderStatusList,
                PaymentStatusList = paymentStatusList
            };
            return View(orderHeaderVM2);
        }


        #region API CALLS

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var allObj = _unitOfWork.OrderHeader.GetAll();
        //    return Json(new { data = allObj });
        //}

        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            var allObj = _unitOfWork.OrderHeader.GetAll(includeProperties:"ApplicationUser");
            IEnumerable<OrderHeader> orderList;
            Debug.WriteLine("Status value: " + status);

            switch (status)
            {
                case "inprocess":
                    orderList = allObj.Where(o => o.OrderStatus == SD.StatusInProcess && o.PaymentStatus != SD.PaymentStatusRejected);
                    break;
                case "pending":
                    orderList = allObj.Where(o => o.OrderStatus == SD.StatusPending && o.PaymentStatus != SD.PaymentStatusRejected);
                    break;
                case "completed":
                    orderList = allObj.Where(o => o.OrderStatus == SD.StatusCompleted && o.PaymentStatus == SD.PaymentStatusApproved);
                    break;
                case "rejected":
                    orderList = allObj.Where(o => o.OrderStatus == SD.StatusCancelled ||
                                                    o.OrderStatus == SD.StatusRefunded ||
                                                    o.PaymentStatus == SD.PaymentStatusRejected);
                    break;
                default:
                    orderList = allObj;
                    break;
            }

            return Json(new { data = orderList });
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

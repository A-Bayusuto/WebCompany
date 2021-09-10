using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models.ViewModels;
using KnifeCompany.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KnifeCompany.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            //var userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName

            // For ASP.NET Core >= 5.0
            //var userEmail = User.FindFirstValue(ClaimTypes.Email) // will give the user's Email
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new Models.OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value)
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value,
                includeProperties:"Company");

            foreach (var list in ShoppingCartVM.ListCart)
            {
                //var productPrice = list.Product.Price;
                //var productQty = list.Count;
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price);
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                list.Product.Description = SD.ConvertToRawHtml(list.Product.Description);
                if (list.Product.Description.Length > 100)  // limiting length of description to 100 characters
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }

            return View(ShoppingCartVM);
        }
    }
}

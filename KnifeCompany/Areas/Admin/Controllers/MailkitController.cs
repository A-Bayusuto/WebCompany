//using KnifeCompany.Models;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using MimeKit;
//using MimeKit.Text;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;

//namespace KnifeCompany.Controllers
//{
//    public class MailkitController : Controller
//    {
//        private readonly ILogger<MailkitController> _logger;
//        private readonly AppSettings _appSettings;
//        private readonly IEmailSender _emailSender;

//        public MailkitController(ILogger<MailkitController> logger, IOptions<AppSettings> appSettings, IEmailSender emailSender)
//        {
//            _logger = logger;
//            _appSettings = appSettings.Value;
//            _emailSender = emailSender;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }


//        [HttpPost]
//        public IActionResult Index(EmailModel model)
//        {
//            // create email message
//            _emailSender.SendEmailAsync("bayusuto@gmail.com", "Settings in JSON file", "Test");
//            ViewBag.Message = "Email Sent Successfully";
//            return View();

//        }


//    }
//}

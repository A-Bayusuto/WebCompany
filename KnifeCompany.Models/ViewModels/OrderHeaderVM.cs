using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnifeCompany.Models.ViewModels
{
    public class OrderHeaderVM
    {

        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetails> OrderDetailsList { get; set; }
        public IEnumerable<SelectListItem> OrderStatusList { get; set; }
        public IEnumerable<SelectListItem> PaymentStatusList { get; set; }


    }
}

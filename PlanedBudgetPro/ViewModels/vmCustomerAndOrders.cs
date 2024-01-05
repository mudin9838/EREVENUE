using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanedBudgetPro.ViewModels
{
    public class vmCustomerAndOrders
    {
        public vmCustomer Customer { get; set; }
       
        public List<vmOrder> Orders { get; set; }
    }
}
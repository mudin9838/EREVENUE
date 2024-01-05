using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanedBudgetPro.ViewModels
{
    public class vmOrder
    {
        public System.Guid OrderId { get; set; }
      //  public string ProductName { get; set; }
     //   public int Quantity { get; set; }
        //public decimal Price { get; set; }
        public int Id { get; set; }
        public string ChartOfAccountId { get; set; }
        public decimal Amountyplan { get; set; }
        public decimal Amountyperformance { get; set; }
        public string RevenueCategoryId { get; set; }
  
        public System.Guid CustomerId { get; set; }



    }
}
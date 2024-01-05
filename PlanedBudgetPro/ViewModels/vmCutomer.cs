using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlanedBudgetPro.ViewModels
{
    public class vmCustomer
    {
        public System.Guid CustomerId { get; set; }
        public string ParentId { get; set; }
        public string ChildId { get; set; }
        public string FisicalYearId { get; set; }
        public string StatusId { get; set; }
        public string ApprovedBy { get; set; }

        public string MonthId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/mm/yyyy}")]
        public System.DateTime OrderDate { get; set; }

    }
}
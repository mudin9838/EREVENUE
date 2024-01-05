using PlanedBudgetPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanedBudgetPro.ViewModels
{
    public class vmChild
    {
       

        public string ChildId { get; set; }
        public string ChildName { get; set; }
        public string ParentId { get; set; }

        public virtual Parent Parent { get; set; }
       
    }
}

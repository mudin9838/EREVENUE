using PlanedBudgetPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanedBudgetPro.ViewModels
{
    public class vmParent
    {
      
        public string ParentId { get; set; }
        public string ParentName { get; set; }

        public virtual ICollection<Child> Children { get; set; }
    }
}

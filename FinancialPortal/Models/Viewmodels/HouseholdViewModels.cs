using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Models.Viewmodels
{
    public class HouseholdDetailsViewModel : BaseViewModel
    {
        public Household Household { get; set; }
    }

    public class NewHouseholdEditModel : BaseViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and no more than {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Household Name")]
        public string HouseholdName { get; set; }
        public string BudgetName { get; set; }
    }

    public class InviteHouseholdViewModel : BaseViewModel
    {
        public int InviteHouseholdId { get; set; }
        public string UserId { get; set; }
        public SelectList UserList { get; set; }
    }

    public class AcceptViewModel : BaseViewModel
    {
        public Household Household { get; set; }
        public Invite Invite { get; set; }
        public bool Accept { get; set; }
    }
}
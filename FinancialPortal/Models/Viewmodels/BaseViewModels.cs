using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.Viewmodels
{
    public class BaseViewModel
    {
        public int HouseholdId { get; set; }
        public string DisplayFirstName { get; set; }
        public string DisplayLastName { get; set; }
        public string DisplayFullName { get; set; }
        public ICollection<Invite> Invites { get; set; }
        public ICollection<Alert> Alerts { get; set; }
    }

    public class AmountViewModel
    {
        [RegularExpression(@"^\-?\d+\.\d{0,2}$", ErrorMessage = "Must be a 2-digit decimal value.")]
        [Range(-999999999.99, 999999999.99, ErrorMessage = "Cannot exceed $1B in magnitude.")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        public AmountViewModel(decimal amount)
        {
            Amount = amount;
        }
        public AmountViewModel() { }
    }
}
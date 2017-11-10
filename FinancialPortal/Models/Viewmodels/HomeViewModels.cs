using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.Viewmodels
{
    public class HomeViewModel : BaseViewModel
    {
        public Household Household { get; set; }
        public ICollection<BankAccountViewModel> BankAccounts { get; set; }
    }

    public class AccountBalanceEditor
    {
        [RegularExpression(@"^\-?\d+\.\d{0,2}$", ErrorMessage = "Must be a 2-digit decimal value.")]
        [Range(-999999999.99, 999999999.99, ErrorMessage = "Cannot exceed $1B in magnitude.")]
        [Display(Name = "Balance")]
        public decimal BankAccountBalance { get; set; }
    }

    public class BankAccountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public string AccountType { get; set; }
        public string Owner { get; set; }
        public string Balance { get; set; }
        public AccountBalanceEditor BalanceEditor { get; set; }
        public BankAccountDetailsViewModel Details { get; set; }
        public HashSet<TransactionViewModel> Transactions { get; set; }
        public CreateTransactionViewModel TransactionCreator { get; set; }
    }
}
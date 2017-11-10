using FinancialPortal.Models.PlaidModels;
using System.Collections.Generic;

namespace FinancialPortal.Models.Viewmodels
{
    public class BanksViewModel : BaseViewModel
    {
        public ICollection<Bank> BankNames { get; set; }
        public LogoBank LogoBank { get; set; }
    }

    public class Bank
    {
        public string BankName { get; set; }
    }
}
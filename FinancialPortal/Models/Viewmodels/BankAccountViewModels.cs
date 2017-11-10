using FinancialPortal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FinancialPortal.Models.Viewmodels
{
    public class CreateBankAccountViewModel : BaseViewModel
    {
        [StringLength(20, ErrorMessage = "Must exceed {2} and cannot exceed {1} characters.", MinimumLength = 6)]
        [Display(Name = "Nickname")]
        public string NickName { get; set; }

        [RegularExpression(@"^\-?\d+\.\d{0,2}$", ErrorMessage = "Must be a 2-digit decimal value.")]
        [Range(-999999999.99, 999999999.99, ErrorMessage = "Cannot exceed $1B in magnitude.")]
        [Display(Name = "Balance")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal BankAccountBalance { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? BankAccountTypeSelection { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? BankNameSelection { get; set; }

        public Dictionary<int, string> BankAccountTypeList { get; set; }

        public SelectList BankNameList { get; set; }

        public CreateBankAccountViewModel()
        {
            BankAccountTypeList = new Dictionary<int, string>();
        }
    }
    public class EditBankAccountViewModel : BaseViewModel
    {
        public EditBankAccountViewModel() { }
        public EditBankAccountViewModel(IEnumerable<BankName> bankNames, IEnumerable<BankAccountType> bankTypes)
        {
            BankNameList = new SelectList(bankNames, "Id", "Name", null);
            BankAccountTypeList = new SelectList(bankTypes, "Id", "Name", null);
        }
        public EditBankAccountViewModel(IEnumerable<BankName> bankNames, int bankNameId, IEnumerable<BankAccountType> bankTypes)
        {
            BankNameList = new SelectList(bankNames, "Id", "Name", bankNameId);
            BankAccountTypeList = new SelectList(bankTypes, "Id", "Name", null);
        }
        public EditBankAccountViewModel(IEnumerable<BankName> bankNames, IEnumerable<BankAccountType> bankTypes, int bankAccountTypeId)
        {
            BankNameList = new SelectList(bankNames, "Id", "Name", null);
            BankAccountTypeList = new SelectList(bankTypes, "Id", "Name", bankAccountTypeId);
        }
        public EditBankAccountViewModel(IEnumerable<BankName> bankNames, IEnumerable<BankAccountType> bankTypes, int bankNameId, int bankAccountTypeId)
        {
            BankNameList = new SelectList(bankNames, "Id", "Name", bankNameId);
            BankAccountTypeList = new SelectList(bankTypes, "Id", "Name", bankAccountTypeId);
        }

        public string FormTitle { get; set; }
        [StringLength(20, ErrorMessage = "Must exceed {2} and cannot exceed {1} characters.", MinimumLength = 6)]
        [Display(Name = "Nickname")]
        public string NickName { get; set; }
        [Required(ErrorMessage = "Required")]
        public int BankAccountTypeSelection { get; set; }

        [Required(ErrorMessage = "Required")]
        public int BankNameSelection { get; set; }

        public SelectList BankNameList { get; set; }
        public SelectList BankAccountTypeList { get; set; }
    }
    public class BankAccountDetailsViewModel
    {
        public BankAccountDetailsViewModel() { }
        public BankAccountDetailsViewModel(IFormatProvider formatProvider, decimal balance, string nickName, string bankName, string bankType, string ownerName, int id, HashSet<TransactionViewModel> transactions)
        {
            Balance = balance.ToString("C", formatProvider);
            HtmlDivId = String.Format("bank-account-details-{0}", id);
            Nickname = nickName;
            BankName = bankName;
            BankType = bankType;
            OwnerName = ownerName;
            Transactions = transactions;
            Properties = new Dictionary<string, string>();
            Properties.Add("Balance", balance.ToString("C", formatProvider));
            Properties.Add("Nickname", nickName);
            Properties.Add("BankName", bankName);
            Properties.Add("BankType", bankType);
            Properties.Add("OwnerName", ownerName);
        }

        public Dictionary<string,string> Properties { get; set; }
        public string Nickname { get; set; }
        public string Balance { get; set; }
        public string BankName { get; set; }
        public string BankType { get; set; }
        public string OwnerName { get; set; }
        public string HtmlDivId { get; set; }
        public HashSet<TransactionViewModel> Transactions { get; set; }
    }
}

//public ActionResult Edit()
//{
//}
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Edit()
//{
//}
//public ActionResult Delete()
//{
//}
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Delete()
//{
//}
//public ActionResult UpdateBalance()
//{
//}
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult UpdateBalance()
//{
//}
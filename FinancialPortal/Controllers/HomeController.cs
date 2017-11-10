using FinancialPortal.Models;
using FinancialPortal.Models.Attributes;
using FinancialPortal.Models.HelperModels;
using FinancialPortal.Models.Viewmodels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    public class HomeController : BaseController
    {
        [AuthorizeInHousehold]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            HomeViewModel vm = new HomeViewModel();
            vm.Household = db.Households.Find(User.Identity.GetHouseholdId());
            vm.BankAccounts = new List<BankAccountViewModel>();
            var accounts = db.BankAccounts.Where(a => a.Owner.HouseholdId == vm.Household.Id).ToList();
            foreach (var account in accounts)
            {
                var type = db.BankAccountTypes.Find(account.BankAccountTypeId).Name;
                var bankName = db.BankNames.Find(account.BankNameId).Name;
                var owner = db.Users.Find(account.OwnerId).FullName;
                vm.BankAccounts.Add(new BankAccountViewModel
                {
                    Id = account.Id,
                    Name = account.NickName,
                    AccountType = type,
                    BankName = bankName,
                    Owner = owner,
                    Balance = account.Balance.ToString("C", ExtensionHelpers.GetCulture(User.Identity.GetCultureId(), User.Identity.GetNegFormatId())),
                    BalanceEditor = new AccountBalanceEditor { BankAccountBalance = account.Balance },
                    Details = GetBankAccountDetails(account.Id),
                    TransactionCreator = GetTransactionCreator(account)
                });
            }
            return View(vm);
        }

        public BankAccountDetailsViewModel GetBankAccountDetails(int? id)
        {
            if (id != null)
            {
                var account = db.BankAccounts.Find(id);
                if (account != null)
                {
                    return new BankAccountDetailsViewModel(
                        ExtensionHelpers.GetCulture(User.Identity.GetCultureId(), User.Identity.GetNegFormatId()),
                        account.Balance,
                        account.NickName,
                        db.BankNames.Find(account.BankNameId).Name,
                        db.BankAccountTypes.Find(account.BankAccountTypeId).Name,
                        db.Users.Find(account.OwnerId).FullName,
                        id.Value,
                        GetBankAccountTransactions(account)
                        );
                }
            }
            return null;
        }

        public HashSet<TransactionViewModel> GetBankAccountTransactions(BankAccount account)
        {
            HashSet<TransactionViewModel> transactions = new HashSet<TransactionViewModel>();
            foreach (var transaction in account.Transactions)
            {
                transaction.Category.Name = db.Categories.Find(transaction.CategoryId).Name;
                transactions.Add(new TransactionViewModel(
                    transaction,
                    ExtensionHelpers.GetCulture(User.Identity.GetCultureId(), User.Identity.GetNegFormatId()),
                    db.Categories.ToList()
                    ));
            }
            return transactions;
        }

        public CreateTransactionViewModel GetTransactionCreator(BankAccount account)
        {
            return new CreateTransactionViewModel(
                db.Categories.ToList()
                );
        }

        public EditTransactionViewModel GetTransactionEditor(BankAccountTransaction transaction)
        {
            return new EditTransactionViewModel(
                db.Categories.ToList(),
                transaction
                );
        }
    }
}
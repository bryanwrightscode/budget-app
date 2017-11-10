using FinancialPortal.Models;
using FinancialPortal.Models.Attributes;
using FinancialPortal.Models.HelperModels;
using FinancialPortal.Models.Viewmodels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static FinancialPortal.Models.Viewmodels.CreateBankAccountViewModel;

namespace FinancialPortal.Controllers
{
    [AuthorizeCustom]
    public class BankAccountsController : BaseController
    {
        public ActionResult Create()
        {
            CreateBankAccountViewModel vm = new CreateBankAccountViewModel
            {
                BankAccountTypeList = new Dictionary<int, string>(),
                BankNameList = new SelectList(db.BankNames.ToList(), "Id", "Name", null)
            };
            foreach (var type in db.BankAccountTypes.ToList())
            {
                vm.BankAccountTypeList.Add(type.Id, type.Name);
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateBankAccountViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                foreach (var _type in db.BankAccountTypes.ToList())
                {
                    vm.BankAccountTypeList.Add(_type.Id, _type.Name);
                }
                vm.BankNameList = new SelectList(db.BankNames.ToList(), "Id", "Name", vm.BankNameSelection);
                return View(vm);
            }
            var nickname = (vm.NickName == null || string.IsNullOrWhiteSpace(vm.NickName)) ? "My Default Account" : vm.NickName;
            var type = db.BankAccountTypes.Find(vm.BankAccountTypeSelection);
            var house = db.Households.Find(User.Identity.GetHouseholdId());
            var user = db.Users.Find(User.Identity.GetUserId());
            var bankName = db.BankNames.Find(vm.BankNameSelection);
            var balance = vm.BankAccountBalance;
            var account = new BankAccount();
            account.NickName = nickname;
            account.BankNameId = bankName.Id;
            account.BankAccountTypeId = type.Id;
            account.OwnerId = user.Id;
            account.Balance = balance;
            db.BankAccounts.Add(account);
            AddAccountToHouseholdUsers(account);
            db.Entry(account).State = EntityState.Added;
            db.SaveChanges();
            await ControllerContext.HttpContext.RefreshAuthentication(user);
            return RedirectToAction("Index", "Home");
        }

        public void AddAccountToHouseholdUsers(BankAccount account)
        {
            foreach (var user in db.Households.Find(User.Identity.GetHouseholdId()).Members)
            {
                user.BankAccounts.Add(account);
                db.Entry(user).State = EntityState.Modified;
            }
        }

        [AuthBankAccount]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                if (db.BankAccounts.Find(id) != null)
                {
                    var account = db.BankAccounts.Find(id);
                    var vm = new EditBankAccountViewModel(
                        db.BankNames.ToList(),
                        db.BankAccountTypes.ToList(),
                        account.BankNameId,
                        account.BankAccountTypeId
                        );
                    vm.NickName = account.NickName;
                    ViewBag.Id = account.Id;
                    return View(vm);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [AuthBankAccount]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBankAccountViewModel vm, int? id)
        {
            if (id != null)
            {
                if (!ModelState.IsValid)
                {
                    vm.BankAccountTypeList = new SelectList(db.BankAccountTypes.ToList(), "Id", "Name", vm.BankAccountTypeSelection);
                    vm.BankNameList = new SelectList(db.BankNames.ToList(), "Id", "Name", vm.BankNameSelection);
                    return View(vm);
                }
                var account = db.BankAccounts.Find(id);
                if (account != null)
                {
                    account.NickName = vm.NickName;
                    account.BankNameId = vm.BankNameSelection;
                    account.BankAccountTypeId = vm.BankAccountTypeSelection;
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [AuthBankAccount]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (id != null)
            {
                var account = db.BankAccounts.Find(id);
                if (account != null)
                {
                    db.BankAccounts.Remove(account);
                    db.Entry(account).State = EntityState.Deleted;
                    user.BankAccounts.Remove(account);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewData["Test"] = "OMG";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [AuthBankAccount]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBalance(int? id, AccountBalanceEditor vm)
        {
            if (id != null)
            {
                var account = db.BankAccounts.Find(id);
                if (account != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    account.Balance = vm.BankAccountBalance;
                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewData["BalanceSuccess"] = "Your account balance was successfully updated";
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public ActionResult UpdateBalanceAjax(string )
        //{
        //    Dictionary<string, string> dictionary = new Dictionary<string, string>();
        //    MyObject request = JsonConvert.DeserializeObject<MyObject>(obj);
        //    var account = db.BankAccounts.Find(request.Id);
        //    if (account != null)
        //    {
        //        account.Balance = request.Value;
        //        db.Entry(account).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return Json(new { Message = "Good", JsonRequestBehavior.AllowGet });
        //    }
        //    return Json(new { Message = "Bad", JsonRequestBehavior.AllowGet });
        //}
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult UpdateBalance()
        ////{
        ////}
        //public class MyObject
        //{
        //    [JsonProperty("Id")]
        //    public int Id { get; set; }
        //    [JsonProperty("Value")]
        //    public decimal Value { get; set; }
        //}
    }

}
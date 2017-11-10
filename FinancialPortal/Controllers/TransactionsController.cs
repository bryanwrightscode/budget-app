using FinancialPortal.Models;
using FinancialPortal.Models.Attributes;
using FinancialPortal.Models.Viewmodels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    public class TransactionsController : BaseController
    {
        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthTransaction]
        public ActionResult Create(int? id, CreateTransactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            else
            {
                if (id != null)
                {
                    var account = db.BankAccounts.Find(id);
                    if (account != null)
                    {
                        account.Transactions.Add(
                            new BankAccountTransaction
                            {
                                Amount = vm.Amount.Amount,
                                Added = DateTimeOffset.Now,
                                CategoryId = vm.CategorySelection,
                                Reconciled = vm.Reconciled,
                                BankAccountId = account.Id,
                                Posted = (vm.Posted.HasValue) ? vm.Posted.Value : vm.Posted
                            });
                        if (vm.Posted.HasValue)
                        {
                            
                        }
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthTransaction]
        public ActionResult Edit(int? id, EditTransactionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            else
            {
                if (id != null)
                {
                    var transaction = db.BankAccountTransactions.Find(id);
                    if (transaction != null)
                    {
                        transaction.Amount = vm.Amount.Amount;
                        transaction.Posted = (vm.Posted.HasValue) ? vm.Posted.Value : vm.Posted;
                        transaction.CategoryId = vm.CategorySelection;
                        transaction.Reconciled = vm.Reconciled;
                        db.Entry(transaction).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Transactions/Delete/5
        [HttpPost]
        [AuthTransaction]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var transaction = db.BankAccountTransactions.Find(id);
                if (transaction != null)
                {
                    db.BankAccountTransactions.Remove(transaction);
                    db.Entry(transaction).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
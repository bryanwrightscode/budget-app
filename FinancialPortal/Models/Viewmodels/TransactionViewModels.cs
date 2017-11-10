using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Models.Viewmodels
{
    public class TransactionViewModel
    {
        public TransactionViewModel(BankAccountTransaction transaction, IFormatProvider formatProvider)
        {
            Id = transaction.Id;
            Amount = transaction.Amount.ToString("C", formatProvider);
            Category = transaction.Category.Name;
            Added = transaction.Added.Value.ToString("d");
            Posted = (transaction.Posted.HasValue) ? transaction.Posted.Value.ToString("d") : "";
            Reconciled = transaction.Reconciled.ToString();
        }

        public TransactionViewModel(BankAccountTransaction transaction, IFormatProvider formatProvider, ICollection<Category> categories)
        {
            Id = transaction.Id;
            Amount = transaction.Amount.ToString("C", formatProvider);
            Category = transaction.Category.Name;
            Added = transaction.Added.Value.ToString("d");
            Posted = (transaction.Posted.HasValue) ? transaction.Posted.Value.ToString("d") : "";
            Reconciled = transaction.Reconciled.ToString();
            TransactionEditor = new EditTransactionViewModel(categories, transaction);
        }

        public int Id { get; set; }
        public string Amount { get; set; }
        public string Category { get; set; }
        public string Added { get; set; }
        public string Posted { get; set; }
        public string Reconciled { get; set; }
        public EditTransactionViewModel TransactionEditor { get; set; }
    }
    public class CreateTransactionViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public AmountViewModel Amount { get; set; }
        [Display(Name = "Date Posted")]
        public DateTimeOffset? Posted { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategorySelection { get; set; }
        public SelectList CategoryList { get; set; }
        public bool Reconciled { get; set; }
        public CreateTransactionViewModel() { }
        public CreateTransactionViewModel(IEnumerable<Category> categories)
        {
            CategoryList = new SelectList(categories, "Id", "Name", null);
        }
    }

    public class EditTransactionViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public AmountViewModel Amount { get; set; }
        [Display(Name = "Date Posted")]
        public DateTimeOffset? Posted { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategorySelection { get; set; }
        public SelectList CategoryList { get; set; }
        public bool Reconciled { get; set; }
        public EditTransactionViewModel() { }
        public EditTransactionViewModel(IEnumerable<Category> categories, BankAccountTransaction transaction)
        {
            CategoryList = new SelectList(categories, "Id", "Name", transaction.CategoryId);
            Amount = new AmountViewModel(transaction.Amount);
            Posted = (transaction.Posted.HasValue) ? transaction.Posted.Value : transaction.Posted;
            Reconciled = transaction.Reconciled;
        }
    }
}
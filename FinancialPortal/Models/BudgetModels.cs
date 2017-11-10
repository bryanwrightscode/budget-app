using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }

    public class Invite
    {
        public int Id { get; set; }
        public string InviteText { get; set; }
        public bool Accepted { get; set; }
        public bool Declined { get; set; }
        public int? HouseholdId { get; set; }
        public string InviterId { get; set; }
        public string InviteeId { get; set; }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser Inviter { get; set; }
        public virtual ApplicationUser Invitee { get; set; }
    }

    public class Alert
    {
        public int Id { get; set; }
        public string AlertText { get; set; }
        public bool Read { get; set; }
        public string AlerteeId { get; set; }
        public int? InviteId { get; set; }

        public virtual ApplicationUser Alertee { get; set; }
        public virtual Invite Invite { get; set; }
    }

    public class Household
    {
        public Household()
        {
            Members = new HashSet<ApplicationUser>();
            Invites = new HashSet<Invite>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? BudgetId { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
    }

    public class BudgetItem
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncome { get; set; }
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual Category Category { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BankAccount
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public int BankAccountTypeId { get; set; }
        public string OwnerId { get; set; }
        public int BankNameId { get; set; }
        public decimal Balance { get; set; }

        public virtual BankAccountType BankAccountType { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual BankName BankName { get; set; }
        public virtual ICollection<BankAccountTransaction> Transactions { get; set; }
    }

    public class BankAccountTransaction
    {
        public int Id { get; set; }
        public DateTimeOffset? Added { get; set; }
        public DateTimeOffset? Posted { get; set; }
        public decimal Amount { get; set; }
        public int BankAccountId { get; set; }
        public int CategoryId { get; set; }
        public bool Reconciled { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        public virtual Category Category { get; set; }
    }

    public class BankName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BankAccountType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }  
}
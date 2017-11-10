using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using System;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;
using System.Globalization;

namespace FinancialPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("HouseholdId", HouseholdId.ToString()));
            userIdentity.AddClaim(new Claim("FullName", FullName));
            userIdentity.AddClaim(new Claim("NegFormatId", NegFormatId.ToString()));
            userIdentity.AddClaim(new Claim("CultureId", CultureId.ToString()));
            foreach (var account in BankAccounts)
            {
                userIdentity.AddClaim(new Claim("BankAccount", account.Id.ToString()));
            }
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public int NegFormatId { get; set; }
        public int CultureId { get; set; }
        public int? HouseholdId { get; set; }
        public virtual Household Household { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
        public virtual ICollection<Alert> Alerts { get; set; }

        public ApplicationUser()
        {
            BankAccounts = new HashSet<BankAccount>();
            Invites = new HashSet<Invite>();
            Alerts = new HashSet<Alert>();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountTransaction> BankAccountTransactions { get; set; }
        public DbSet<BankName> BankNames { get; set; }
        public DbSet<BankAccountType> BankAccountTypes { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}
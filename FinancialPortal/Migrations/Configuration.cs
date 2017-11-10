namespace FinancialPortal.Migrations
{
    using FinancialPortal.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FinancialPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FinancialPortal.Models.ApplicationDbContext context)
        {
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.BankAccountTypes.AddOrUpdate(
                t => t.Name,
                new BankAccountType { Name = "Checking" },
                new BankAccountType { Name = "Savings" }
                );
            context.BankNames.AddOrUpdate(
                n => n.Name,
                new BankName { Name = "PNC" },
                new BankName { Name = "Wells Fargo"},
                new BankName { Name = "US Bank" }
                );
        }
    }
}

using FinancialPortal.Models;
using FinancialPortal.Models.Attributes;
using FinancialPortal.Models.HelperModels;
using FinancialPortal.Models.Viewmodels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    [RequireHttps]
    public class BaseController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResult;
            if (result != null)
            {
                if (result.Model != null)
                {
                    var baseViewModel = result.Model as BaseViewModel;
                    if (Request.IsAuthenticated)
                    {
                        ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                        baseViewModel.Invites = user.Invites.Where(i => i.Accepted == false).Where(i => i.HouseholdId != user.HouseholdId).ToList();
                        baseViewModel.DisplayFirstName = user.FirstName;
                        baseViewModel.DisplayLastName = user.LastName;
                        baseViewModel.DisplayFullName = user.FullName;
                        var alerts = user.Alerts.Where(a => a.AlerteeId == user.Id);
                        baseViewModel.Alerts = alerts.OrderByDescending(a => a.Id).ToList();
                        if (user.HouseholdId.HasValue)
                            baseViewModel.HouseholdId = user.HouseholdId.Value;
                    }
                }
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.UrlReferrer != null)
            {
                ViewData["BackUrl"] = filterContext.RequestContext.HttpContext.Request.UrlReferrer.ToString();
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
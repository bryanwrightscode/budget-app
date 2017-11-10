using FinancialPortal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FinancialPortal.Models.Attributes
{
    public class AuthorizeCustom : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Register" }));
        }
    }
}
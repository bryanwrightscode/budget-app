using FinancialPortal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FinancialPortal.Models.Attributes
{
    public class AuthorizeInHousehold : AuthorizeAttribute
    {
        //Overrides AuthorizeAttribute class
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext)) //Still check authorization
            {
                return false;
            }
            return httpContext.User.Identity.IsInHousehold(); //User authorized if this succeeds
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult
                    (new RouteValueDictionary
                    (new { controller = "Households", action = "Create" }));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
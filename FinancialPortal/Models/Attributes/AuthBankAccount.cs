using FinancialPortal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace FinancialPortal.Models.Attributes
{
    public class AuthBankAccount : AuthorizeInHousehold
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext))
            {
                return false;
            }
            else
            {
                var routeData = httpContext.Request.RequestContext.RouteData;
                var routeId = routeData.Values["id"] as string;
                if (routeId != null)
                {
                    int number;
                    return (Int32.TryParse(routeId, out number))
                        ? ((httpContext.User.Identity.OwnsBankAccount(number)) ? true : false)
                        : false;
                }
                else
                {
                    return false;
                }
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult
                    (new RouteValueDictionary
                    (new { controller = "Home", action = "Index" }));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult
                    (new RouteValueDictionary
                    (new { controller = "Account", action = "Register" }));
            }
        }
    }
}
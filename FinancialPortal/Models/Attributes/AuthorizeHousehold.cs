using FinancialPortal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FinancialPortal.Models.Attributes
{
    /*
     * This is a custom authorization attribute that overrides the existing Web.MVC one.
     * In addition to checking that the user is signed in, it checks the custom HouseholdId claim.
     * If a claim of that type is missing the result is false.
     * If one exists but the value is null, the result is false.
     * If one exists but the value does not match the route value, the result is false.
     * If one exists and the value matches the route value, the result is true.
     * Future work should remove routing household id and instead rely on the claim.
     * This filter can be repurposed for use in bank accounts, budgets, and transactions.
     */
    public class AuthorizeHousehold : AuthorizeAttribute
    {
        //Overrides AuthorizeAttribute class
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext)) //Still check authorization
            {
                return false;
            }
            else
            {
                var hId = httpContext.User.Identity.GetHouseholdId();
                var routeData = httpContext.Request.RequestContext.RouteData;
                var routeHouseholdIdString = routeData.Values["id"] as string;
                if (httpContext.User.Identity.IsInHousehold())
                {
                    return httpContext.User.Identity.IsInThisHousehold(routeHouseholdIdString); //User authorized if this succeeds
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
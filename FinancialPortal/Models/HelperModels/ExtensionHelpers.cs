using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FinancialPortal.Models.HelperModels
{
    public static class ExtensionHelpers
    {

        public static CultureInfo GetCulture(int cultureId, int negFormatId)
        {
            CultureInfo inf = new CultureInfo(cultureId, true);
            inf.NumberFormat.CurrencyNegativePattern = negFormatId;
            var culture = CultureInfo.CurrentCulture.LCID;
            return inf;
        }

        public static bool OwnsBankAccount(this IIdentity user, int? id)
        {
            if (id != null)
            {
                var claimsIdentity = (ClaimsIdentity)user;
                var bankClaims = claimsIdentity.Claims.Where(c => c.Type == "BankAccount").ToList();
                if (bankClaims != null)
                {
                    if (bankClaims.Count() > 0)
                    {
                        if (bankClaims.All(b => b.Value != null && !string.IsNullOrWhiteSpace(b.Value)))
                        {
                            if (bankClaims.Any(c => Int32.Parse(c.Value) == id.Value))
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool CanAccessTransaction(this IIdentity user, int? id)
        {
            if (id != null)
            {
                var claimsIdentity = (ClaimsIdentity)user;
                ApplicationDbContext db = new ApplicationDbContext();
                if (db.Households.Find(user.GetHouseholdId()).Members.SelectMany(m => m.BankAccounts).Any(t => t.Id == id))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user; //Cast; IIdentity user type converted to ClaimsIdentity type
            var HouseholdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseholdId"); //Gets claim with this name
            if (HouseholdClaim != null)
            {
                var claimId = HouseholdClaim.Value;
                if (claimId != null && !string.IsNullOrWhiteSpace(claimId))
                    return Int32.Parse(HouseholdClaim.Value);
                else
                    return null;
            }
            else
                return null;
        }

        public static int GetCultureId(this IIdentity user)
        {
            var cIdentity = (ClaimsIdentity)user;
            var cultureId = cIdentity.Claims.FirstOrDefault(c => c.Type == "CultureId");
            if (cultureId == null)
                return 0;
            else
            {
                return (cultureId.Value != null && !string.IsNullOrWhiteSpace(cultureId.Value))
                    ? Int32.Parse(cultureId.Value)
                    : 0;
            }
        }

        public static int GetNegFormatId(this IIdentity user)
        {
            var cIdentity = (ClaimsIdentity)user;
            var formatId = cIdentity.Claims.FirstOrDefault(c => c.Type == "NegFormatId");
            if (formatId == null)
                return 0;
            else
            {
                return (formatId.Value != null && !string.IsNullOrWhiteSpace(formatId.Value))
                    ? Int32.Parse(formatId.Value)
                    : 0;
            }
        }

        public static string GetFullName(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var FullNameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "FullName");
            return FullNameClaim.Value;
        }

        public static bool IsInHousehold(this IIdentity user)
        {
            var cUser = (ClaimsIdentity)user;
            var hId = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            return (hId != null && !string.IsNullOrWhiteSpace(hId.Value)); //Returns true if claim has value
        }

        public static bool IsInThisHousehold(this IIdentity user, string routeId)
        {
            var cUser = (ClaimsIdentity)user;
            var householdClaim = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            if (householdClaim != null)
            {
                var claimId = householdClaim.Value;
                if (!string.IsNullOrWhiteSpace(claimId))
                {
                    if (claimId == routeId)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
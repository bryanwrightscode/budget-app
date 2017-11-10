using FinancialPortal.Models;
using FinancialPortal.Models.Attributes;
using FinancialPortal.Models.HelperModels;
using FinancialPortal.Models.Viewmodels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    [AuthorizeCustom]
    public class HouseholdsController : BaseController
    {
        [AuthorizeHousehold]
        public ActionResult Details(int? id)
        {
            HouseholdDetailsViewModel vm = new HouseholdDetailsViewModel();
            vm.Household = db.Households.Find(id);
            var user = db.Users.Find(User.Identity.GetUserId());
            return View(vm);
        }

        public ActionResult Create()
        {
            NewHouseholdEditModel em = new NewHouseholdEditModel();
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.HouseholdId == null)
            {
                return View(em);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewHouseholdEditModel em)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                if (user.HouseholdId == null)
                {
                    var household = new Household();
                    household.Name = em.HouseholdName;
                    household.Members.Add(user);
                    db.Households.Add(household);
                    user.HouseholdId = household.Id;
                    db.SaveChanges();
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(em);
        }

        [HttpPost]
        public async Task<ActionResult> Leave(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var household = db.Households.Find(id);
            if (household != null)
            {
                if (user.HouseholdId == household.Id &&
                    household.Members.Any(m => m.Id == user.Id))
                {
                    household.Members.Remove(user);
                    user.HouseholdId = null;
                    var alerts = user.Alerts.Where(a => a.InviteId == null ||
                    (a.Invite.HouseholdId == household.Id));
                    db.Alerts.RemoveRange(alerts);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    await LeaveAlert(user.FullName);
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task LeaveAlert(string leaverName)
        {
            var household = db.Households.Find(User.Identity.GetHouseholdId());
            foreach (var member in household.Members)
            {
                Alert alert = new Alert();
                alert.AlerteeId = member.Id;
                alert.AlertText = String.Format("{0} left your household", leaverName);
                db.Alerts.Add(alert);
                await db.SaveChangesAsync();
            }
        }

        public ActionResult ReadAlert(int id)
        {
            var alert = db.Alerts.Find(id);
            alert.Read = true;
            db.Entry(alert).State = EntityState.Modified;
            db.SaveChanges();
            if (alert.InviteId != null)
            {
                var invite = db.Invites.Find(alert.InviteId);
                if (invite.InviteeId == User.Identity.GetUserId())
                {
                    return RedirectToAction("Accept", "Invites", new { id = invite.Id });
                }
                else
                {
                    return RedirectToAction("Details", "Households", new { id = invite.HouseholdId.Value });
                }
            }
            else
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                return RedirectToAction("Details", "Households", new { id = user.HouseholdId.Value });
            }
        }

        public string MarkAllAlertsRead()
        {
            var alerts = db.Users.Find(User.Identity.GetUserId()).Alerts.Where(a => a.Read == false).ToList();
            foreach (var alert in alerts)
            {
                alert.Read = true;
                db.Entry(alert).State = EntityState.Modified;
            }
            db.SaveChanges();
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary["message"] = "ok";
            string json = JsonConvert.SerializeObject(dictionary);
            return json;
        }
    }
}
using FinancialPortal.Models;
using FinancialPortal.Models.HelperModels;
using FinancialPortal.Models.Viewmodels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class InvitesController : BaseController
    {
        public ActionResult Accept(int? id)
        {
            if (id != null)
            {
                AcceptViewModel vm = new AcceptViewModel();
                vm.Invite = db.Invites.Find(id);
                if (vm.Invite != null)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    if (user.Invites.Any(i => i.Id == vm.Invite.Id && i.Accepted == false && i.InviteeId == vm.Invite.InviteeId) && vm.Invite.InviteeId == user.Id && vm.Invite.Accepted == false)
                    {
                        vm.Household = db.Households.Find(vm.Invite.HouseholdId);
                        return View(vm);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Accept")]
        public async Task<ActionResult> Accept(AcceptViewModel vm)
        {
            var invite = db.Invites.Find(vm.Invite.Id);
            var household = db.Households.Find(vm.Household.Id);
            if (invite != null && household != null)
            {
                if (db.Users.Find(User.Identity.GetUserId()).Invites.Any(i => i.Id == invite.Id) && invite.Accepted == false)
                {
                    invite.Accepted = true;
                    var invitee = db.Users.Find(invite.InviteeId);
                    if (invitee.HouseholdId != null)
                    {
                        db.Households.Find(invitee.HouseholdId).Members.Remove(invitee);
                    }
                    var alerts = invitee.Alerts.Where(a => a.InviteId == null ||
                    (a.Invite.Accepted == true && a.Invite.InviteeId == invitee.Id) ||
                    (a.Invite.Declined == true && a.Invite.InviteeId == invitee.Id));
                    db.Alerts.RemoveRange(alerts);
                    invitee.HouseholdId = invite.HouseholdId;
                    household.Members.Add(invitee);
                    invitee.Invites.Remove(invite);
                    db.Entry(invite).State = EntityState.Modified;
                    db.SaveChanges();
                    var members = household.Members.Where(m => m.Id != invitee.Id);
                    foreach (var member in members)
                    {
                        await AcceptAlert(member.Id, invitee.FullName, invite.Id);
                    }
                    await AcceptAlert(invitee.Id, invite.Id, invite.Inviter.FullName);
                    await ControllerContext.HttpContext.RefreshAuthentication(invitee);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Decline")]
        public async Task<ActionResult> Decline(AcceptViewModel vm)
        {
            var invite = db.Invites.Find(vm.Invite.Id);
            var household = db.Households.Find(vm.Household.Id);
            if (invite != null && household != null)
            {
                if (db.Users.Find(User.Identity.GetUserId()).Invites.Any(i => i.Id == invite.Id) && invite.Accepted == false)
                {
                    invite.Declined = true;
                    var invitee = db.Users.Find(invite.InviteeId);
                    invitee.Invites.Remove(invite);
                    var alerts = invitee.Alerts.Where(a => a.InviteId == null ||
                    (a.Invite.Accepted == true && a.Invite.InviteeId == invitee.Id) ||
                    (a.Invite.Declined == true && a.Invite.InviteeId == invitee.Id));
                    db.Alerts.RemoveRange(alerts);
                    db.Entry(invite).State = EntityState.Modified;
                    db.SaveChanges();
                    await DeclineAlert(invite.InviterId, invitee.FullName, invite.Id);
                    await ControllerContext.HttpContext.RefreshAuthentication(invitee);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Invite()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            InviteHouseholdViewModel vm = new InviteHouseholdViewModel();
            vm.InviteHouseholdId = user.Household.Id;
            var users = db.Users.Where(u =>
            !u.Invites.Any(i => i.Accepted == false && i.HouseholdId == user.HouseholdId)
            )
            .Where(u => u.HouseholdId != user.HouseholdId)
            .ToList();
            if (users.Count() > 0)
            {
                vm.UserList = new SelectList(users.OrderBy(u => u.LastName), "Id", "UserName", vm.UserId);
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invite(InviteHouseholdViewModel vm)
        {
            var invite = new Invite();
            invite.InviterId = User.Identity.GetUserId();
            invite.InviteeId = vm.UserId;
            invite.HouseholdId = vm.InviteHouseholdId;
            invite.Household = db.Households.Find(vm.HouseholdId);
            var invitee = db.Users.Find(invite.InviteeId);
            invitee.Invites.Add(invite);
            invite.InviteText = String.Format("{0} invited you to join their household.", db.Users.Find(invite.InviterId).FullName);
            db.SaveChanges();
            await InviteAlert(User.Identity.GetFullName(), invite.InviteeId, invite.Id);
            return RedirectToAction("Index", "Home");
        }

        public async Task InviteAlert(string inviterName, string inviteeId, int inviteId)
        {
            Alert alert = new Alert();
            alert.AlerteeId = inviteeId;
            alert.AlertText = String.Format("{0} invited you to join their household", inviterName);
            alert.InviteId = inviteId;
            db.Alerts.Add(alert);
            await db.SaveChangesAsync();
        }

        public async Task AcceptAlert(string inviterId, string inviteeName, int inviteId)
        {
            Alert alert = new Alert();
            alert.AlerteeId = inviterId;
            alert.AlertText = String.Format("{0} joined your household", inviteeName);
            alert.InviteId = inviteId;
            db.Alerts.Add(alert);
            await db.SaveChangesAsync();
        }

        public async Task AcceptAlert(string inviteeId, int inviteId, string inviterName)
        {
            Alert alert = new Alert();
            alert.AlerteeId = inviteeId;
            alert.AlertText = String.Format("You joined {0}'s household", inviterName);
            alert.InviteId = inviteId;
            db.Alerts.Add(alert);
            await db.SaveChangesAsync();
        }

        public async Task DeclineAlert(string inviterId, string inviteeName, int inviteId)
        {
            Alert alert = new Alert();
            alert.AlerteeId = inviterId;
            alert.AlertText = String.Format("{0} declined your invitation to join your household", inviteeName);
            alert.InviteId = inviteId;
            db.Alerts.Add(alert);
            await db.SaveChangesAsync();
        }
    }
}
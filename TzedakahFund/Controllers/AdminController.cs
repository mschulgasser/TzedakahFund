using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TzedakahFund.Data;
using TzedakahFund.Models;

namespace TzedakahFund.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            if (!manager.GetUser(User.Identity.Name).IsAdmin)
            {
                return Redirect("/home/login");
            }

            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            var vm = new AdminIndexViewModel();
            vm.Categories = repo.GetCategories();
            //vm.PendingApplications = repo.GetApplications(Status.Pending);
            return View(vm);
        }
        [Authorize]
        public ActionResult Categories()
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            if (!manager.GetUser(User.Identity.Name).IsAdmin)
            {
                return Redirect("/home/login");
            }
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            var vm = new CategoriesViewModel();
            vm.Categories = repo.GetCategories();
            return View(vm);
        }
        [HttpPost]
        public void AddCategory(string name)
        {
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            repo.AddCategory(name);
        }
        
        public ActionResult GetCategories()
        {
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            var categories = repo.GetCategories().Select(c => new
            {
                name = c.Name,
                id = c.Id,
                hasApplications = c.Applications.Count > 0
            }).ToList();
            
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditCategory(string name, int id)
        {
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            repo.EditCategory(name,id);
            return Redirect("/admin/getcategories");
        }
        [HttpPost]
        public void DeleteCategory(int id)
        {
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            repo.DeleteCategory(id);
        }
        public ActionResult GetPending()
        {
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            var pendingApplications = repo.GetApplications(Status.Pending).Select(a =>
                new { id = a.Id, firstName = a.User.FirstName, lastName = a.User.LastName, email = a.User.Email,
                amount = a.Amount.ToString("C"), categoryName = a.Category.Name});
            return Json(pendingApplications, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void AcceptApplication(int id)
        {
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            repo.AcceptApplication(id);
        }
        [HttpPost]
        public void RejectApplication(int id)
        {
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            repo.RejectApplication(id);
        }
        [Authorize]
        public ActionResult ViewHistory(string email)
        {
            return Redirect("/home/viewhistory?email=" + email);
        }
    }
}
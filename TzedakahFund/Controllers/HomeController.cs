using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TzedakahFund.Data;
using TzedakahFund.Models;

namespace TzedakahFund.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            if (User.Identity.IsAuthenticated)
            {
                if (manager.GetUser(User.Identity.Name).IsAdmin)
                {
                    return Redirect("/admin/index");
                }
                return Redirect("/home/manage");
            }
            return View();
        }
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(string email, string password)
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            User u = manager.Login(email, password);
            if(u != null)
            {
                FormsAuthentication.SetAuthCookie(email, true);
                if (u.IsAdmin)
                {
                    return Redirect("/admin/index");
                }
                return Redirect("/home/manage");
            }
            return Redirect("/home/login");
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string email, string password)
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            manager.AddUser(firstName, lastName, email, password);
            FormsAuthentication.SetAuthCookie(email, true);
            return Redirect("/home/manage");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        [Authorize]
        public ActionResult Apply()
        {
            var vm = new ApplicationViewModel();
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            vm.Categories = repo.GetCategories();
            vm.User = manager.GetUser(User.Identity.Name);
            return View(vm);
        }
        [Authorize][HttpPost]
        public ActionResult Apply(Application a)
        {
            var repo = new TzedakahFundRepository(Properties.Settings.Default.ConStr);
            repo.AddApplication(a);
            return Redirect("/home/manage");
        }
        [Authorize]
        public ActionResult Manage()
        {
            return View();
        }
        [Authorize] 
        //public ActionResult ViewHistory()
        //{
        //    var vm = new ViewHistoryViewModel();
        //    var manager = new UserManager(Properties.Settings.Default.ConStr);
        //    vm.User = manager.GetUser(User.Identity.Name);
        //    return View(vm);
        //}
        [Authorize]
        public ActionResult ViewHistory(string email)
        {
            var vm = new ViewHistoryViewModel();
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            if(email == "" || email == null)
            {
                vm.User = manager.GetUser(User.Identity.Name);
            }
            else
            {
                vm.User = manager.GetUser(email);
            }
            return View(vm);
        }
        public ActionResult UserExists(string email)
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            return Json(new { userExists = manager.UserExists(email) }, JsonRequestBehavior.AllowGet);
        }
    }
}
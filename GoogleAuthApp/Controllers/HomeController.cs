using GoogleAuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoogleAuthApp.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public async Task<ActionResult> SearchUser(string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
                return View("~/Home/Index.cshtml");
                 
            else { 
            var db = new ApplicationDbContext();
            var users = from u in db.Users
                         select new
                         {
                             UserId = u.Id,
                             UserName = u.UserName,
                             FirstName = u.FirstName,
                             SecondName = u.SecondName
                         };
            var db2 = new PictureContext();
            var images = from p in db2.Pictures
                         select p;
            List<SearchUserViewModel> usersList = new List<SearchUserViewModel>();

            users = users.Where(s => s.UserName.Contains(searchString));
            foreach(var u in users)
            {
                 var pictures = (from p in images
                         where p.UserId == u.UserId
                         select p.Image).ToList();
                 if (pictures.Count() != 0)
                 {   
                  usersList.Add(new SearchUserViewModel (u.UserId, u.UserName, u.FirstName, u.SecondName, pictures[0]));
                 }
                 else
                 {
                     usersList.Add(new SearchUserViewModel(u.UserId, u.UserName, u.FirstName, u.SecondName, new byte[0]));
                 }
            }
            ViewBag.List = usersList;
            return View();
            }
            
        }
    }
}
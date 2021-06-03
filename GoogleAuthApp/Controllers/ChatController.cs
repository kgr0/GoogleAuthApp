using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoogleAuthApp.Models;
using Microsoft.AspNet.Identity;

namespace GoogleAuthApp.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        MessageContext db = new MessageContext();
        ApplicationDbContext dbUser = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        /*[HttpGet]
        public ActionResult Chat()
        {
            ViewBag.userId = "";
            ViewBag.userName = "";
            return View();
        }*/
 
        [HttpGet]
        public ActionResult CheckChatGroupAndOpenChatWith(string userId = "")
        {
            var myUserId = User.Identity.GetUserId();
            var chatGroup = db.ChatModels.Where(u => (u.IdUser1.Equals(myUserId) && u.IdUser2.Equals(userId)) || (u.IdUser1.Equals(userId) && u.IdUser2.Equals(myUserId))).ToArray();
            if(chatGroup.Count() == 0)
            {
                db.ChatModels.Add(new ChatModel {IdUser1 = myUserId, IdUser2 = userId });
                db.SaveChanges();
            }
            return RedirectToAction("Chat","Chat", new { userId = userId });
        }
        [HttpGet]
        public ActionResult Chat(string userId = "")
        {
            if (userId.Equals(""))
            {
                ViewBag.userId = "";
                ViewBag.userName = "";
            }
            else
            {
                var user = dbUser.Users.Where(u => u.Id.Equals(userId)).First();
                ViewBag.userId = user.Id;
                ViewBag.userName = user.UserName;
            }

            return View();
        }
    }
}

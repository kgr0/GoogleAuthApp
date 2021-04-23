using GoogleAuthApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data.Entity.Validation;

namespace GoogleAuthApp.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
       public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowOwnProfile()
        {
            var db = new ApplicationDbContext();
            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Where(u => u.Id.Equals(userId)).Single();
        
            ViewBag.FirstName = user.FirstName;
            ViewBag.SecondName = user.SecondName;
            ViewBag.UserName = user.UserName;
            ViewBag.Photo = user.Photo;
            ViewBag.BirthDay = user.BirthDay;
            return RedirectToAction("Index2");
            return View();
        }
        /*
        [HttpPost]
        public ActionResult Create(ApplicationUser item, HttpPostedFileBase ImageFile)
        {
            Response.Write("<script>alert('item.Email');</script>");
            using (var ms = new MemoryStream())
            {
                ImageFile.InputStream.CopyTo(ms);
                item.Photo = ms.ToArray();
            }
            var db = new ApplicationDbContext();
            Console.WriteLine(item.Email);
            

            if (ModelState.IsValid)
            {
                db.Users.Add(item);
            }
                return View();
        }

            [HttpPost]
        public ActionResult Createe(ApplicationUser item, HttpPostedFileBase ImageFile)
        {
            Response.Write("<script>alert('111111111111111 text');</script>");

            // return Redirect("/Profile/Create");
            using (var ms = new MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    item.Photo = ms.ToArray();
                }
                var db = new ApplicationDbContext();

                if (ModelState.IsValid)
                {
                    db.Users.Add(item);

            try
            {
              
                    db.SaveChanges();

                  
            }
            catch (DbEntityValidationException e)
            {
                    foreach (DbEntityValidationResult validationError in e.EntityValidationErrors)
                    {
                        Response.Write("Object: " + validationError.Entry.Entity.ToString());
                        Response.Write(" ");
                            foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            Response.Write(err.ErrorMessage + " ");
                        }
                    }
                }  Response.Write("<script>alert('Your text');</script>");

                    return RedirectToAction("Create");
                }
                return View(item);
        }
        */


        PictureContext db = new PictureContext();

        public ActionResult Index2()
        {
            return View(db.Pictures);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PictureViewModel pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                pic.Image = imageData;

                db.Pictures.Add(pic);
                db.SaveChanges();

                return RedirectToAction("ShowOwnProfile");
            }
            return View(pic);
        }

    }
}
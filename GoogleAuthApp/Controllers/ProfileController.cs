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
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;

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
            ViewBag.BirthDay = user.BirthDay.ToShortDateString();//  .ToLongDateString();

            PictureContext db2 = new PictureContext();
            if (db2.Pictures.Where(u => u.UserId.Equals(userId)).Count() != 0)
            {
                ViewBag.Photo = db2.Pictures.Where(u => u.UserId.Equals(userId)).Single().Image;
            }
            else
            {
                ViewBag.Photo = new byte[0];
            }
            var model = new ShowPostsModel
            {
                PostsAndOffers = ShowPostsAndOffersAtProfileForUser(userId)
            };
            return View(model);
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
        public ActionResult ShowProfile(string userId)
        {
            var db = new ApplicationDbContext();
            ApplicationUser user = db.Users.Where(u => u.Id.Equals(userId)).Single();

            ViewBag.FirstName = user.FirstName;
            ViewBag.SecondName = user.SecondName;
            ViewBag.UserName = user.UserName;
            ViewBag.BirthDay = user.BirthDay.ToShortDateString();//  .ToLongDateString();
            ViewBag.UserId = userId;

            PictureContext db2 = new PictureContext();
            if (db2.Pictures.Where(u => u.UserId.Equals(userId)).Count() != 0)
            {
                ViewBag.Photo = db2.Pictures.Where(u => u.UserId.Equals(userId)).Single().Image;
            }
            else
            {
                ViewBag.Photo = new byte[0];
            }
            ViewBag.Check = "Profile";
            var model = new ShowPostsModel
            {
                PostsAndOffers = ShowPostsAndOffersAtProfileForUser(userId)
            };

            return View(model);
        }


        public ActionResult Index2()
        {
            PictureContext db = new PictureContext();
            string userId = User.Identity.GetUserId();
            return View(db.Pictures.Where(u => u.UserId.Equals(userId)));
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
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                PictureContext db = new PictureContext();
                string userId = User.Identity.GetUserId();
                if (db.Pictures.Where(u => u.UserId.Equals(userId)).Count() != 0)
                {
                    var picture = db.Pictures.Where(u => u.UserId.Equals(userId)).Single();
                    picture.Image = imageData;
                }
                else
                {
                    pic.Image = imageData;
                    pic.UserId = User.Identity.GetUserId();

                    db.Pictures.Add(pic);
                }
                db.SaveChanges();

                return RedirectToAction("ShowOwnProfile");
            }
            return View(pic);
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            var db = new PostOfferContext();
            var dbCategories = new ApplicationDbContext();
            db.PostModels.Add(new PostModel { Text = "Your text", Date = DateTime.Now, UserId = User.Identity.GetUserId(), Likes = 0 });
            db.SaveChanges();
            var model = new CreatePostModel
            {
                Id = db.PostModels.Max(p=> p.Id),
                Text = "Your text",
                Date = DateTime.Now,
                GroupsOfInterests = dbCategories.GroupsOfInterests.ToList()
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult CreatePost(CreatePostModel createPostModel)
        {
            var db = new PostOfferContext();
            var post = db.PostModels.Where(p => p.Id == createPostModel.Id).First();
            post.Text = createPostModel.Text;
            db.SaveChanges();

            return RedirectToAction("ShowOwnProfile");
        }
        public ActionResult AddCategoryToPost(int postId, string categoryId)
        {
            var categoryIdInt = int.Parse(categoryId);
            var db = new PostOfferContext();
            if(db.PostCatergoryModels.Where(c=> c.PostId == postId && c.CategoryId == categoryIdInt).Count()==0)
            {
                db.PostCatergoryModels.Add(new PostCategoryModel { CategoryId = categoryIdInt, PostId = postId });
                db.SaveChanges();
            }

            return RedirectToAction("EditPost", new {Id = postId });
        }

        [HttpGet]
        public ActionResult EditPost(int Id)
        {
            var db = new PostOfferContext();
            var dbCategories = new ApplicationDbContext();
            var post = db.PostModels.Where(p => p.Id == Id).First();
            var model = new CreatePostModel
            {
                Id = Id,
                Text = post.Text,
                Categories = GetPostCategories(Id),
                Date = post.Date,
                PostPictures = GetPostPictures(Id),
                GroupsOfInterests = dbCategories.GroupsOfInterests.ToList()

            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPost(CreatePostModel createPostModel)
        {
            var db = new PostOfferContext();
            var post = db.PostModels.Where(p => p.Id == createPostModel.Id).First();
            post.Text = createPostModel.Text;
            db.SaveChanges();

            return RedirectToAction("ShowOwnProfile");
        }
        public ActionResult DeletePost(int postId)
        {
            var db = new PostOfferContext();
            var post = db.PostModels.Where(p => p.Id == postId).First();
            var categories = db.PostCatergoryModels.Where(c => c.PostId == postId).ToList();
            foreach (var category in categories)
            {
                db.PostCatergoryModels.Remove(category);
            }
            db.PostModels.Remove(post);

            db.SaveChanges();

            return RedirectToAction("ShowOwnProfile");
        }
        public ActionResult DeleteCategoryFromPost(int postId, int postCategoryId)
        {
            var db = new PostOfferContext();

            if (db.PostCatergoryModels.Where(c => c.Id == postCategoryId).Count() != 0)
            {
                var del = db.PostCatergoryModels.Where(c => c.Id == postCategoryId).First();
                db.PostCatergoryModels.Remove(del);
                db.SaveChanges();
            }

            return RedirectToAction("EditPost", new { Id = postId });
        }
        public ActionResult AddLikeToPost(int postId, string backTo, string profileUserId )
        {
            var db = new PostOfferContext();
            var post = db.PostModels.Where(p => p.Id == postId).First();
            var userId = User.Identity.GetUserId();
            if (db.PostLikes.Where(l => l.PostId == postId && l.UserId.Equals(userId)).Count() == 0)
            {
                db.PostLikes.Add(new PostLike { UserId = userId, PostId = postId });
                post.Likes = post.Likes + 1;
                db.SaveChanges();
            }

            if (backTo.Equals("Offers"))
                return RedirectToAction("Offers");
            else if (backTo.Equals("ShowProfile"))
                return RedirectToAction("ShowProfile", new { userId = profileUserId });
            else
                return RedirectToAction("ShowOwnProfile");
        }
        [HttpPost]
        public ActionResult AddPictureToPost(CreatePostModel createPostModel, PostPicture pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                var db = new PostOfferContext();
                int postId = createPostModel.Id;
                if (db.PostPictures.Where(u => u.PostId.Equals(postId) && u.Picture.Equals(imageData)).Count() == 0)
                {
                    pic.Picture = imageData;
                    pic.PostId = postId;
                    db.PostPictures.Add(pic);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("EditPost", new { Id = createPostModel.Id });
        }
        /*-----------------------------------------------------------------------------------------------------------------------------------------------------------*/
        [HttpGet]
        public ActionResult CreateOffer()
        {
            var db = new PostOfferContext();
            var dbCategories = new ApplicationDbContext();
            db.OfferModels.Add(new OfferModel { Text = "Your text", Date = DateTime.Now, UserId = User.Identity.GetUserId(), Likes = 0 });
            db.SaveChanges();
            var model = new CreatePostModel
            {
                Id = db.OfferModels.Max(p => p.Id),
                Text = "Your text",
                Date = DateTime.Now,
                GroupsOfInterests = dbCategories.GroupsOfInterests.ToList()
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult CreateOffer(CreatePostModel createOfferModel)
        {
            var db = new PostOfferContext();
            var offer = db.OfferModels.Where(p => p.Id == createOfferModel.Id).First();
            offer.Text = createOfferModel.Text;
            db.SaveChanges();

            return RedirectToAction("ShowOwnProfile");
        }
        public ActionResult AddCategoryToOffer(int offerId, string categoryId)
        {
            var categoryIdInt = int.Parse(categoryId);
            var db = new PostOfferContext();
            if (db.OfferCatergoryModels.Where(c => c.OffreId == offerId && c.CategoryId == categoryIdInt).Count() == 0)
            {
                db.OfferCatergoryModels.Add(new OfferCategoryModel { CategoryId = categoryIdInt,  OffreId = offerId });
                db.SaveChanges();
            }

            return RedirectToAction("EditOffer", new { Id = offerId });
        }
        [HttpGet]
        public ActionResult EditOffer(int Id)
        {
            var db = new PostOfferContext();
            var dbCategories = new ApplicationDbContext();
            var offer = db.OfferModels.Where(p => p.Id == Id).First();
            var model = new CreatePostModel
            {
                Id = Id,
                Text = offer.Text,
                Categories = GetOfferCategories(Id),
                Date = offer.Date,
                OfferPictures = GetOfferPictures(Id),
                GroupsOfInterests = dbCategories.GroupsOfInterests.ToList()

            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditOffer(CreatePostModel createPostModel)
        {
            var db = new PostOfferContext();
            var offer = db.OfferModels.Where(p => p.Id == createPostModel.Id).First();
            offer.Text = createPostModel.Text;
            db.SaveChanges();

            return RedirectToAction("ShowOwnProfile");
        }
        public ActionResult DeleteOffer(int offerId)
        {
            var db = new PostOfferContext();
            var offer = db.OfferModels.Where(p => p.Id == offerId).First();
            var categories = db.OfferCatergoryModels.Where(c => c.OffreId == offerId).ToList();
            foreach (var category in categories)
            {
                db.OfferCatergoryModels.Remove(category);
            }
            db.OfferModels.Remove(offer);

            db.SaveChanges();

            return RedirectToAction("ShowOwnProfile");
        }
        public ActionResult DeleteCategoryFromOffer(int offerId, int offerCategoryId)
        {
            var db = new PostOfferContext();

            if (db.OfferCatergoryModels.Where(c => c.Id == offerCategoryId).Count() != 0)
            {
                var del = db.OfferCatergoryModels.Where(c => c.Id == offerCategoryId).First();
                db.OfferCatergoryModels.Remove(del);
                db.SaveChanges();
            }

            return RedirectToAction("EditOffer", new { Id = offerId });
        }
        public ActionResult AddLikeToOffer(int offerId, string backTo, string profileUserId)
        {
            var db = new PostOfferContext();
            var offer = db.OfferModels.Where(p => p.Id == offerId).First();
            var userId = User.Identity.GetUserId();
            if(db.OfferLikes.Where(l => l.OfferId == offerId && l.UserId.Equals(userId)).Count() == 0)
            {
                db.OfferLikes.Add(new OfferLike { UserId = userId, OfferId = offerId });
                offer.Likes = offer.Likes + 1;
                db.SaveChanges();
            }

            if (backTo.Equals("Offers"))
                return RedirectToAction("Offers");
            else if (backTo.Equals("ShowProfile"))
                return RedirectToAction("ShowProfile", new { userId = profileUserId});
            else
                return RedirectToAction("ShowOwnProfile");
        }
        [HttpPost]
        public ActionResult AddPictureToOffer(CreatePostModel createOfferModel, OfferPicture pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                var db = new PostOfferContext();
                int offerId = createOfferModel.Id;
                if (db.OfferPictures.Where(u => u.OfferId.Equals(offerId) && u.Picture.Equals(imageData)).Count() == 0)
                {
                    pic.Picture = imageData;
                    pic.OfferId= offerId;
                    db.OfferPictures.Add(pic);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("EditOffer", new { Id = createOfferModel.Id });
        }

        public List<CreatePostModel> ShowPostsAndOffersAtProfileForUser(string userId)
        {
            List<CreatePostModel> backList = new List<CreatePostModel>();
            var db = new PostOfferContext();

            var allUserPosts = db.PostModels.Where(u => u.UserId.Equals(userId)).ToList();
            foreach (var post in allUserPosts)
            {
                var postCategories = GetPostCategories(post.Id);
                var postPictures = GetPostPictures(post.Id);
                backList.Add(new CreatePostModel { Id = post.Id, Text = post.Text, Categories = postCategories, Date = post.Date, Likes = post.Likes, UserId = userId, PostOrOffer = "Post", PostPictures  = postPictures });
            }

            var allUserOffers = db.OfferModels.Where(u => u.UserId.Equals(userId)).ToList();
            foreach (var offer in allUserOffers)
            {
                var offerCategories = GetOfferCategories(offer.Id);
                var offerPictures = GetOfferPictures(offer.Id);
                backList.Add(new CreatePostModel { Id = offer.Id, Text = offer.Text, Categories = offerCategories, Date = offer.Date, Likes = offer.Likes, UserId = userId, PostOrOffer = "Offer", OfferPictures = offerPictures });
            }

            return backList;
        }

        /* --------------------------------------------------------------------------------------------------------------------------------*/
        void  Function1(List<int> hobbies, List<int> keys, List<int> counts,int weight)
        {
            //var hobbies = new List<int>();    // lista zainteresowań użytkownika ;
            foreach(var hobby in hobbies)
            {
                if(!keys.Contains(hobby))
                {
                    keys.Add(hobby);
                    counts.Add(0);
                }
                var i= keys.FindIndex(x=> x == hobby);
                counts[i] += weight;
                //Sort(keys, counts, i);
            }
        }
        void Recomendation()
        {
            var keys = new List<int>();
            var counts = new List<int>();

            /*Function1(list, keys, counts, 10);
            Function1(list, keys, counts, 3);
            Function1(list, keys, counts, 2);
            Function1(list, keys, counts, 1);*/

            var result = Scale(counts);
        }
        public List<float> Scale( List<int> counts)
        {
            var countsFloat = new List <float>();
            var sum = counts.Sum();
            foreach (var i in counts)
            {
                countsFloat.Add(((float)i * 100) / (float)sum);
            }
            return countsFloat;
        }
        /* --------------------------------------------------------------------------------------------------------------------------------*/
        public List<CreatePostModel> ShowAllOffers()
        {
            List<CreatePostModel> backList = new List<CreatePostModel>();
            var db = new PostOfferContext();

            var allUserOffers = db.OfferModels.ToList();
            foreach (var offer in allUserOffers)
            {
                var offerCategories = GetOfferCategories(offer.Id);
                var offerPictures = GetOfferPictures(offer.Id);
                backList.Add(new CreatePostModel { Id = offer.Id, Text = offer.Text, Categories = offerCategories, Date = offer.Date, Likes = offer.Likes, UserId = offer.UserId, PostOrOffer = "Offer", OfferPictures = offerPictures });
            }

            return backList;
        }
        public ActionResult Offers()
        {
            var model = new ShowOffersModel
            {
                Offers = ShowAllOffers()
            };
            return View(model);
        }

        #region Helpers
        public List<GroupsOfInterest> GetPostCategories(int Id)
        {
            var db = new PostOfferContext();
            var dbCategories = new ApplicationDbContext();
            var Categories = dbCategories.GroupsOfInterests;

            var postCategorie= db.PostCatergoryModels.Where(p=>p.PostId == Id).ToList();
            List<GroupsOfInterest> backList = new List<GroupsOfInterest>();

            foreach (var p in postCategorie)
            {
                var a = Categories.Where(g => g.Id == p.CategoryId).First();
                a.Id = p.Id; // to get ID from PostCategoryModels in View
                backList.Add(a);
            }
            return backList;
        }
        public List<PostPicture> GetPostPictures(int Id)
        {
            var db = new PostOfferContext();
            return db.PostPictures.Where(p => p.PostId == Id).ToList();
        }
        public List<OfferPicture> GetOfferPictures(int Id)
        {
            var db = new PostOfferContext();
            return db.OfferPictures.Where(p => p.OfferId == Id).ToList();
        }
        public List<GroupsOfInterest> GetOfferCategories(int Id)
        {
            var db = new PostOfferContext();
            var dbCategories = new ApplicationDbContext();
            var Categories = dbCategories.GroupsOfInterests;

            var offerCategories = db.OfferCatergoryModels.Where(p => p.OffreId == Id).ToList();
            List<GroupsOfInterest> backList = new List<GroupsOfInterest>();

            foreach (var o in offerCategories)
            {
                var a = Categories.Where(g => g.Id == o.CategoryId).First();
                a.Id = o.Id; // to get ID from PostCategoryModels in View
                backList.Add(a);
            }
            return backList;
        }
        #endregion
    }
}
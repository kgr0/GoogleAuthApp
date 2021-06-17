using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace GoogleAuthApp.Models
{
    public class CreatePostModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<GroupsOfInterest> Categories { get; set; }
        public DateTime Date { get; set; }
        public int Likes { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PostOrOffer { get; set; }
        public List<PostPicture> PostPictures { get; set; }
        public List<OfferPicture> OfferPictures { get; set; }
        public List<GroupsOfInterest> GroupsOfInterests { get; set; }
    }
    public class ShowPostsModel
    {
        public List<CreatePostModel> PostsAndOffers { get; set; }
        public bool Follow { get; set; }
        public List<GroupsOfInterest> Categories { get; set; }
        public int FollowersAmount { get; set; }
        public int PostsAmount { get; set; }
        public int OffersAmount { get; set; }
    }
    public class ShowOffersModel
    {
        public List<CreatePostModel> Offers { get; set; }
    }
    public class FilterModel
    {
        public OfferModel Offer { get; set; }
        public float Weight { get; set; }
    }
    public class RecomendationModel
    {
        public string Category { get; set; }
        public float Weight { get; set; }
    }


    public class PostModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int Likes { get; set; }
    }
    public class OfferModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int Likes { get; set; }
    }
    public class PostCategoryModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int PostId { get; set; }
    }
    public class OfferCategoryModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int OffreId { get; set; }
    }
    public class OfferLike
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int OfferId { get; set; }
    }
    public class PostLike
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
    }
    public class PostPicture
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public byte[] Picture { get; set; }
    }
    public class OfferPicture
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public byte[] Picture { get; set; }
    }




    public class PostOfferContext : DbContext
    {
        public PostOfferContext()
            : base("DefaultConnection")
        { }

        public DbSet<PostModel> PostModels { get; set; }
        public DbSet<PostCategoryModel> PostCatergoryModels { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostPicture> PostPictures { get; set; }
        public DbSet<OfferModel> OfferModels { get; set; }
        public DbSet<OfferCategoryModel> OfferCatergoryModels { get; set; }
        public DbSet<OfferLike> OfferLikes { get; set; }
        public DbSet<OfferPicture> OfferPictures { get; set; }
    }
}
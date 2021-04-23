using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GoogleAuthApp.Models
{
    public class OwnProfileViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Second Name")]
        public string SecondName { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public byte[] Photo { get; set; }

        public DateTime BirthDay { get; set; }
    }

    public class PictureViewModel
    {
        [Key]
        public string UserId { get; set; }
        public byte[] Image { get; set; }
    }

    public class PictureContext : DbContext
    {
        public PictureContext()
            : base("DefaultConnection")
        { }

        public DbSet<PictureViewModel> Pictures { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoogleAuthApp.Models
{
    public class Follow 
    {
        public int Id { get; set; }
        public string IdUser1 { get; set; }
        public string IdUser2 { get; set; }
    }
}
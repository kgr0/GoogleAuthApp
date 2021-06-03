using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAuthApp.Models
{
    public class ChosenGroup
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GroupId { get; set; }
    }
}
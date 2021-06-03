using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAuthApp.Models
{
    public class GroupsOfInterest
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public int ParentId { get; set; }
    }
}
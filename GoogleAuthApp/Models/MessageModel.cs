using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAuthApp.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public bool ReadFrom { get; set; }
        public bool ReadTo { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleAuthApp.Models
{
    public class ChatGroupModel
    {
        public string ConnectionId { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool Read { get; set; }

    }
}
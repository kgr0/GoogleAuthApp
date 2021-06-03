using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace GoogleAuthApp.Models
{
    public class MessageContext : DbContext
    {
        public MessageContext() : base("DefaultConnection") { }
        public DbSet<MessageModel> MessageModels { get; set; }
        public DbSet<ChatModel> ChatModels { get; set; }
    }
}
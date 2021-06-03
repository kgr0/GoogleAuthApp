using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoogleAuthApp.Models;
using System.Security.Claims;

namespace GoogleAuthApp
{
    public class ChatHub : Hub
    {
        #region Data Members

        MessageContext db = new MessageContext();
        ApplicationDbContext dbUser = new ApplicationDbContext();
        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static List<ChatGroupModel> ConnectedChatGroup = new List<ChatGroupModel>();

        #endregion
        #region Methods


        public void Connect(string userName, string id)
        {
            ConnectedChatGroup.Clear();
            var connectionId = Context.ConnectionId;
            var chatUsers = db.ChatModels.Where(u => u.IdUser1.Equals(id) || u.IdUser2.Equals(id)).ToList();
            var usersDetail = dbUser.Users;
            var messages = db.MessageModels;
            foreach (var chatUser in chatUsers)
            {
                var chatWithUserId = chatUser.IdUser1;
                if (chatUser.IdUser1.Equals(id))
                    chatWithUserId = chatUser.IdUser2;

                var require = messages.Where(u => (u.From.Equals(id) && u.To.Equals(chatWithUserId)) || (u.From.Equals(chatWithUserId) && u.To.Equals(id))).OrderByDescending(u => u.Time).ToArray();
                var lastMessageStatus = true;

                if (require.Count() != 0)
                {
                    if (require.First().From.Equals(id))
                        lastMessageStatus = require.First().ReadFrom;
                    else
                        lastMessageStatus = require.First().ReadTo;
                }

                if (ConnectedUsers.Count(x => x.Id == chatWithUserId) == 0)
                {
                    ConnectedUsers.Add(new UserDetail { Id = chatWithUserId, UserName = usersDetail.FirstOrDefault(u => u.Id.Equals(chatWithUserId)).UserName, ConnectionId = "111" });
                    ConnectedChatGroup.Add(new ChatGroupModel { Id = chatWithUserId, UserName = usersDetail.FirstOrDefault(u => u.Id.Equals(chatWithUserId)).UserName, ConnectionId = "111", Read = lastMessageStatus });
                }
                else
                {
                    var currUser = ConnectedUsers.FirstOrDefault(x => x.Id.Equals(chatWithUserId));
                    ConnectedChatGroup.Add(new ChatGroupModel { Id = currUser.Id, UserName = currUser.UserName, ConnectionId = currUser.ConnectionId, Read = lastMessageStatus });
                }
            }


            if (ConnectedUsers.Count(x => x.Id == id) == 0)
            {
                ConnectedUsers.Add(new UserDetail { Id = id, UserName = userName, ConnectionId = connectionId });
                ConnectedChatGroup.Add(new ChatGroupModel { Id = id, UserName = userName, ConnectionId = connectionId, Read = true });

                // send to caller
                Clients.Caller.onConnected(id, ConnectedChatGroup);
            }
            else
            {
                var user = ConnectedUsers.FirstOrDefault(u => u.Id == id);
                user.ConnectionId = connectionId;

                var currUser = ConnectedUsers.FirstOrDefault(x => x.Id.Equals(id));
                ConnectedChatGroup.Add(new ChatGroupModel { Id = currUser.Id, UserName = currUser.UserName, ConnectionId = connectionId, Read = true });

                Clients.Caller.onConnected(id, ConnectedChatGroup);
            }

        }

        public void SendPrivateMessage(string toUserId, string toConnectionId, string message, string fromUserId)
        {
            //string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.Id == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.Id == fromUserId);

            if (toUser != null && fromUser != null)
            {
                db.MessageModels.Add(new MessageModel { From = fromUserId, To = toUserId, Message = message, Time = DateTime.Now, ReadFrom = false, ReadTo = false });
                db.SaveChanges();
                // send to 
                //Clients.Client(toConnectionId).messageReceived(fromUser.UserName, message, fromUserId);
                Clients.Client(toConnectionId).messageReceived(fromUser.UserName, message, fromUserId);

                // send to caller user
                Clients.Caller.messageReceived(fromUser.UserName, message, toUserId);
            }
        }
        public void GetChatHistory(string fromUserId, string toUserId)
        {
            var toUser = ConnectedUsers.FirstOrDefault(x => x.Id == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.Id == fromUserId);

            if (toUser != null && fromUser != null)
            {
                var mod = db.MessageModels.Where(u => (u.From.Equals(fromUserId) && u.To.Equals(toUserId)) || (u.From.Equals(toUserId) && u.To.Equals(fromUserId))).OrderBy(u => u.Time);
                foreach (var b in mod)
                    Clients.Caller.messageReceived(ConnectedUsers.FirstOrDefault(x => x.Id == b.From).UserName, b.Message, toUserId);
            }
        }
        public void GetUserInformation(string userId)
        {
            Clients.Caller.onSetUserInformation(ConnectedUsers.FirstOrDefault(x => x.Id == userId));
        }
        public string GetUserConnectionId(string userId)
        {
            return ConnectedUsers.FirstOrDefault(x => x.Id == userId).ConnectionId;
        }
        public void ReadMessages(string fromUserId, string toUserId)
        {
            //Clients.Caller.onAlert("Read");
            var messages = db.MessageModels.Where(u => ((u.From.Equals(fromUserId) && u.To.Equals(toUserId)) || (u.From.Equals(toUserId) && u.To.Equals(fromUserId))) && (u.ReadFrom.Equals(false) || u.ReadTo.Equals(false)));
            foreach (var message in messages)
            {
                if (message.From.Equals(fromUserId))
                    message.ReadFrom = true;
                else
                    message.ReadTo = true;
            }
            db.SaveChanges();
        }

        #endregion
    }
}
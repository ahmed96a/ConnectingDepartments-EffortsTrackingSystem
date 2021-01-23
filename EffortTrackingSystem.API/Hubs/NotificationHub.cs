using EffortTrackingSystem.API.Models.Repository;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Hubs
{
    [Authorize]
    public class NotificationHub: Hub
    {
        private readonly INotificationRepository notificationRepo;

        public NotificationHub(INotificationRepository _notificationRepo)
        {
            notificationRepo = _notificationRepo;
        }

        public async Task UpdateNotifications(string receiverId)
        {
            var newNotificationsCount = notificationRepo.GetNewNotifications(receiverId);
            await Clients.Caller.SendAsync("updateNotificationsBar", newNotificationsCount);
        }

        // call client-side method NewNotification once the user is connected to the server, to display notification count in the notification icon.
        // that method is called each time an http request is sent to the server.
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // that will equal null if we didn't configure the signalR authentication with JWT.
            Clients.User(userId).SendAsync("NewNotification", userId).Wait();
            return base.OnConnectedAsync();
        }
    }
}

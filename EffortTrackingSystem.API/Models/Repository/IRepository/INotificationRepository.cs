using EffortTrackingSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository.IRepository
{
    public interface INotificationRepository
    {
        public bool CreateNotification(Notification notification);

        public int GetNewNotifications(string receiverId);

        public ICollection<Notification> GetNotifications(string receiverId);

        public Notification OpenNotification(int id, string receiverId);

        public bool? MarkAsRead(int id, string receiverId);
    }
}

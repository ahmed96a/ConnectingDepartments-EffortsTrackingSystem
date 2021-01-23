using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext db;

        public NotificationRepository(AppDbContext _db)
        {
            db = _db;
        }

        public bool CreateNotification(Notification notification)
        {
            db.Notifications.Add(notification);
            return db.SaveChanges() > 0 ? true : false;
        }

        public int GetNewNotifications(string receiverId)
        {
            return db.Notifications.Where(x => x.ReceiverId == receiverId && x.IsRead == false).Count();
        }

        public ICollection<Notification> GetNotifications(string receiverId)
        {
            return db.Notifications.Where(x => x.ReceiverId == receiverId).ToList();
        }

        public bool? MarkAsRead(int id, string receiverId)
        {
            var not = db.Notifications.SingleOrDefault(x => x.Id == id && x.ReceiverId == receiverId);
            if (not == null)
                return null;

            not.IsRead = true;
            return db.SaveChanges() > 0 ? true : false;
        }

        public Notification OpenNotification(int id, string receiverId)
        {
            var not = db.Notifications.SingleOrDefault(x => x.Id == id && x.ReceiverId == receiverId);

            if (not != null)
            {
                not.IsRead = true;
                db.SaveChanges();
            }
            return not;
        }
    }
}

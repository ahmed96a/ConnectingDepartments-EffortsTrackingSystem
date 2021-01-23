using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Repository.IRepository
{
    public interface INotificationRepository
    {
        Task<object> GetNotificationsAsync(string Url, string token);

        Task<object> OpenNotificationAsync(string Url, int id, string token);

        Task<object> MarkAsReadAsync(string Url, int id, string token);
    }
}

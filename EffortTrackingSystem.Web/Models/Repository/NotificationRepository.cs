using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<object> GetNotificationsAsync(string Url, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + "GetNotifications");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var notificationDtos = JsonConvert.DeserializeObject<List<NotificationDto>>(responseContent);
                return notificationDtos;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> MarkAsReadAsync(string Url, int id, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"MarkAsRead/{id}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> OpenNotificationAsync(string Url, int id, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"OpenNotification/{id}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var notificationDto = JsonConvert.DeserializeObject<NotificationDto>(responseContent);
                return notificationDto;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }
    }
}

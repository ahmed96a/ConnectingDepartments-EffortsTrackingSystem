using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Repository
{
    public class MissionRepository : IMissionRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MissionRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> AssignTaskToUserAsync(string Url, string userId, int missionId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + $"AssignTaskToUser?userId={userId}&missionId={missionId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null && token.Length > 0)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return null;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllMissionsForDepartmentAsync(string Url, int departmentId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"GetAllMissionsForDepartment/{departmentId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var missionDtos = JsonConvert.DeserializeObject<List<MissionDto>>(responseContent);
                return missionDtos;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllMissionsForCategoryAsync(string Url, int categoryId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"GetAllMissionsForCategory/{categoryId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var missionDtos = JsonConvert.DeserializeObject<List<MissionDto>>(responseContent);
                return missionDtos;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllSendedMissionsOfUserAsync(string Url, string userId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"GetAllSendedMissionsOfUser/{userId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var missionDtos = JsonConvert.DeserializeObject<List<MissionDto>>(responseContent);
                return missionDtos;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllReceivedMissionsOfUserAsync(string Url, string userId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"GetAllReceivedMissionsOfUser/{userId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var missionDtos = JsonConvert.DeserializeObject<List<MissionDto>>(responseContent);
                return missionDtos;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllFilteredMissionsAsync(string Url, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url);

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var missionDtos = JsonConvert.DeserializeObject<List<MissionDto>>(responseContent);
                return missionDtos;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllFilteredMissionsCountAsync(string Url, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url);

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var missionsCountDto = JsonConvert.DeserializeObject<MissionsCountDto>(responseContent);
                return missionsCountDto;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllDepartmentMissionsCountAsync(string Url, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url);

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var missionsCountDto = JsonConvert.DeserializeObject<DepartmentMissionsCountDto>(responseContent);
                return missionsCountDto;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetMissionAsync(string Url, int missionId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"GetMission/{missionId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var missionDto = JsonConvert.DeserializeObject<MissionDto>(responseContent);
                return missionDto;
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

        public async Task<string> SendMissionAsync(string Url, CreateMissionDto createMissionDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url + "SendMission");

            var jsonContent = JsonConvert.SerializeObject(createMissionDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return null;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<string> AcceptMissionAsync(string Url, int missionId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + $"AcceptMission/{missionId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<string> RefuseMissionAsync(string Url, int missionId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + $"RefuseMission/{missionId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<string> CompleteMissionAsync(string Url, int missionId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + $"CompleteMission/{missionId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<string> ApproveMissionAsync(string Url, int missionId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + $"ApproveMission/{missionId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
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

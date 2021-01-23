using EffortTrackingSystem.Models.DtoResponses;
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
    public class UserRepository : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<object> CreateUserAsync(string Url, CreateUserDto createUserDto, string passwordResetLink, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url + $"CreateUser?passwordResetLink={passwordResetLink}");

            var jsonContent = JsonConvert.SerializeObject(createUserDto);
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
            else if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(responseContent);
                return generalResponse;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> DeleteUserAsync(string Url, string userId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, Url + $"DeleteUser/{userId}");
            HttpClient httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(responseContent);
                return generalResponse;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return "User Not Found";
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllUsersAsync(string Url, int? departmentId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"GetAllUsers?departmentId={departmentId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(responseContent);
                return userDtos;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllNonAdminUsersAsync(string Url, int? departmentId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"GetAllNonAdminUsers?departmentId={departmentId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(responseContent);
                return userDtos;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetUserAsync(string Url, string userId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + $"GetUser/{userId}");

            var httpClient = _httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var userDto = JsonConvert.DeserializeObject<UserDto>(responseContent);
                return userDto;
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

        public async Task<object> ResetPasswordAsync(string Url, ResetPasswordDto resetPasswordDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + "ResetPassword");

            var jsonContent = JsonConvert.SerializeObject(resetPasswordDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

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
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(responseContent);
                return generalResponse;
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }
    }
}

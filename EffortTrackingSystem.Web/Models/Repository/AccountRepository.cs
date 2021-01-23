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
    public class AccountRepository : IAccountRepository
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AccountRepository(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<object> LoginAsync(string Url, LoginDto loginDto)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url + "Login");
            var jsonContent = JsonConvert.SerializeObject(loginDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var accountLoginResponse = JsonConvert.DeserializeObject<AccountLoginResponse>(responseContent);
                return accountLoginResponse;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var accountLoginResponse = JsonConvert.DeserializeObject<AccountLoginResponse>(responseContent);
                return accountLoginResponse;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> RegisterAsync(string Url, RegisterDto registerDto)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url + "Register");
            var jsonContent = JsonConvert.SerializeObject(registerDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
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
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAllRolesAsync(string Url, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + "GetAllRoles");

            var httpClient = httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var roleDtos = JsonConvert.DeserializeObject<List<RoleDto>>(responseContent);
                return roleDtos;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return "You are not Authorized to access that resource";
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetRoleAsync(string Url, string roleId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + "GetRole/" + roleId);

            var httpClient = httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var roleDto = JsonConvert.DeserializeObject<RoleDto>(responseContent);
                return roleDto;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return "BadRequest";
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return "Role Not Found";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return "You are not Authorized to access that resource";
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> CreateRoleAsync(string Url, CreateRoleDto createRoleDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url + "CreateRole");
            var jsonContent = JsonConvert.SerializeObject(createRoleDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient();

            if (token != null && token.Length > 0)
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

        public async Task<object> UpdateRoleAsync(string Url, string roleId, UpdateRoleDto updateRoleDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + "UpdateRole/" + roleId);
            request.Content = new StringContent(JsonConvert.SerializeObject(updateRoleDto), Encoding.UTF8, "application/json");

            HttpClient httpClient = httpClientFactory.CreateClient();

            if (token != null && token.Length > 0)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return null;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent))
                    return "Bad Request";

                var generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(responseContent);
                return generalResponse;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> DeleteRoleAsync(string Url, string roleId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, Url + "DeleteRole/" + roleId);
            HttpClient httpClient = httpClientFactory.CreateClient();

            if (token != null && token.Length > 0)
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
                return "Role Not Found";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return "You are not Authorized to access that resource";
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> EditProfileAsync(string Url, EditProfileDto editProfileDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + $"EditProfile/{editProfileDto.Id}");
            request.Content = new StringContent(JsonConvert.SerializeObject(editProfileDto), Encoding.UTF8, "application/json");

            HttpClient httpClient = httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return null;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseContent))
                    return "Bad Request";

                var generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(responseContent);
                return generalResponse;
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }
    }
}

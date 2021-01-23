using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Web.Models.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IHttpClientFactory httpClientFactory;

        public DepartmentRepository(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> CreateAsync(string Url, CreateDepartmentDto createDepartmentDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);
            var jsonContent = JsonConvert.SerializeObject(createDepartmentDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient();

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
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        public async Task<object> GetAllAsync(string Url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url);

            var httpClient = httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var departmentDtos = JsonConvert.DeserializeObject<List<DepartmentDto>>(responseContent);
                return departmentDtos;
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return "You are not Authorized to access that resource";
            }
            else
            {
                string ApiExceptionMessage = await response.Content.ReadAsStringAsync();
                return ApiExceptionMessage;
            }
        }

        public async Task<object> GetAsync(string Url, int DepartmentId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + DepartmentId);

            var httpClient = httpClientFactory.CreateClient();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DepartmentDto>(responseContent);
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        public async Task<string> UpdateAsync(string Url, UpdateDepartmentDto updateDepartmentDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + updateDepartmentDto.Id);

            var jsonContent = JsonConvert.SerializeObject(updateDepartmentDto);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient();

            if (token != null && token.Length > 0)
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
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        public async Task<string> DeleteAsync(string Url, int DepartmentId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, Url + DepartmentId);

            var httpClient = httpClientFactory.CreateClient();

            if (token != null && token.Length > 0)
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
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }
    }
}

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
    public class CategoryRepository : ICategoryRepository
    {
        readonly IHttpClientFactory httpClientFactory;

        public CategoryRepository(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<object> GetAllAsync(string Url, int? departmentId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + "GetAllCategories" + "?departmentId=" + departmentId);

            var httpClient = httpClientFactory.CreateClient();

            if (token != null && token.Length > 0)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var categoryDtos = JsonConvert.DeserializeObject<List<CategoryDto>>(responseContent);
                return categoryDtos;
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

        public async Task<string> CreateAsync(string Url, CreateCategoryDto createCategoryDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);

            var jsonContent = JsonConvert.SerializeObject(createCategoryDto);
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
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        public async Task<string> DeleteAsync(string Url, int CategoryId, string token)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, Url + CategoryId);

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

        public async Task<object> GetAsync(string Url, int CategoryId, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + CategoryId);

            var httpClient = httpClientFactory.CreateClient();

            if (token != null && token.Length > 0)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CategoryDto>(responseContent);
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        public async Task<string> UpdateAsync(string Url, UpdateCategoryDto updateCategoryDto, string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + updateCategoryDto.Id);

            var jsonContent = JsonConvert.SerializeObject(updateCategoryDto);
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
    }
}

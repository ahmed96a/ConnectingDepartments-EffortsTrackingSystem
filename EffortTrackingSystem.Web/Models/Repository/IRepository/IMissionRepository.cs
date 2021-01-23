using EffortTrackingSystem.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.Web.Models.Repository.IRepository
{
    public interface IMissionRepository
    {
        Task<string> AssignTaskToUserAsync(string Url, string userId, int missionId, string token);

        Task<object> GetAllMissionsForDepartmentAsync(string Url, int departmentId, string token);

        Task<object> GetAllMissionsForCategoryAsync(string Url, int categoryId, string token);

        Task<object> GetMissionAsync(string Url, int missionId, string token);

        Task<object> GetAllSendedMissionsOfUserAsync(string Url, string userId, string token);

        Task<object> GetAllReceivedMissionsOfUserAsync(string Url, string userId, string token);

        Task<object> GetAllFilteredMissionsAsync(string Url, string token);

        Task<object> GetAllFilteredMissionsCountAsync(string Url, string token);

        Task<object> GetAllDepartmentMissionsCountAsync(string Url, string token);

        Task<string> SendMissionAsync(string Url, CreateMissionDto createMissionDto, string token);

        Task<string> AcceptMissionAsync(string Url, int missionId, string token);

        Task<string> RefuseMissionAsync(string Url, int missionId, string token);

        Task<string> CompleteMissionAsync(string Url, int missionId, string token);

        Task<string> ApproveMissionAsync(string Url, int missionId, string token);
    }
}

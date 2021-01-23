using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository.IRepository
{
    public interface IMissionRepository
    {
        bool AssignTaskToUser(string userId, int missionId);

        ICollection<Mission> GetAllSendedMissionsOfUser(string userId);

        ICollection<Mission> GetAllReceivedMissionsOfUser(string userId);

        ICollection<Mission> GetAllMissionsForDepartment(int departmentId);

        ICollection<Mission> GetAllMissionsForCategory(int categoryId);

        ICollection<Mission> GetAllFilteredMissions(int? departmentId, int? categoryId, string missionType, string missionState, string userId);

        MissionsCountDto GetAllFilteredMissionsCount(string missionType, string userId);

        DepartmentMissionsCountDto GetAllDepartmentMissionsCount(string departmentId);

        Mission GetMission(int MissionId);

        bool IsMissionExists(int MissionId);

        bool CreateMission(Mission mission);

        bool UpdateMission(Mission mission);

        bool DeleteMission(Mission mission);

        bool save();
    }
}

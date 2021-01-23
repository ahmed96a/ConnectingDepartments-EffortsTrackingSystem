using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.Models.Dtos;
using EffortTrackingSystem.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API.Models.Repository
{
    public class MissionRepository : IMissionRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext db;

        public MissionRepository(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            db = appDbContext;
        }

        public bool AssignTaskToUser(string userId, int MissionId)
        {
            var mission = db.Missions.SingleOrDefault(m => m.Id == MissionId);
            mission.ReceiverId = userId;
            return db.SaveChanges() > 0 ? true : false;
        }

        public bool CreateMission(Mission mission)
        {
            db.Missions.Add(mission);
            return save();
        }

        //----
        public bool DeleteMission(Mission mission)
        {
            db.Missions.Remove(mission);
            return save();
        }

        //----

        // --------------------

        public ICollection<Mission> GetAllSendedMissionsOfUser(string userId)
        {
            return db.Missions.Where(a => a.SenderId == userId).ToList();
        }

        public ICollection<Mission> GetAllReceivedMissionsOfUser(string userId)
        {
            return db.Missions.Where(a => a.ReceiverId == userId).ToList();
        }

        // --------------------

        public ICollection<Mission> GetAllMissionsForCategory(int categoryId)
        {
            return db.Missions.Where(a => a.Category_Id == categoryId).ToList();
        }

        public ICollection<Mission> GetAllMissionsForDepartment(int departmentId)
        {
            return db.Missions.Where(a => a.Category.DepartmentId == departmentId).Include(x => x.Sender).Include(x => x.Receiver).Include(x => x.Category).ToList();
        }

        // missionType => Sended, Received
        public ICollection<Mission> GetAllFilteredMissions(int? departmentId, int? categoryId, string missionType, string missionState, string userId)
        {
            var missionsIQuerable = db.Missions.AsQueryable();
            missionsIQuerable = departmentId != null ? missionsIQuerable.Where(x => x.Category.DepartmentId == departmentId) : missionsIQuerable;

            missionsIQuerable = categoryId != null ? missionsIQuerable.Where(x => x.Category_Id == categoryId) : missionsIQuerable;

            missionsIQuerable = missionType == "Sended" ? missionsIQuerable.Include(x => x.Sender).Include(x => x.Receiver).Where(x => x.SenderId == userId) : missionsIQuerable.Include(x => x.Sender).Include(x => x.Receiver).Where(x => x.ReceiverId == userId);

            if (missionState == "running")
                missionsIQuerable = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == false);
            else if (missionState == "waiting")
                missionsIQuerable = missionsIQuerable.Where(x => x.TaskState == 0);
            else if (missionState == "completed")
                missionsIQuerable = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == true && x.Is_Approved == false);
            else if (missionState == "approved")
                missionsIQuerable = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == true && x.Is_Approved == true);
            else if (missionState == "refused")
                missionsIQuerable = missionsIQuerable.Where(x => x.TaskState == 2);

            return missionsIQuerable.Include(x => x.Category).ToList();
        }

        public MissionsCountDto GetAllFilteredMissionsCount(string missionType, string userId)
        {
            MissionsCountDto missionsCountDto = new MissionsCountDto();

            var missionsIQuerable = db.Missions.AsQueryable();

            missionsIQuerable = missionType == "Sended" ? missionsIQuerable.Where(x => x.SenderId == userId) : missionsIQuerable.Where(x => x.ReceiverId == userId);
            missionsCountDto.RunningCount = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == false).Count();
            missionsCountDto.WaitingCount = missionsIQuerable.Where(x => x.TaskState == 0).Count();
            missionsCountDto.CompletedCount = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == true && x.Is_Approved == false).Count();
            missionsCountDto.ApprovedCount = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == true && x.Is_Approved == true).Count();
            missionsCountDto.RefusedCount = missionsIQuerable.Where(x => x.TaskState == 2).Count();

            return missionsCountDto;
        }


        public DepartmentMissionsCountDto GetAllDepartmentMissionsCount(string departmentId)
        {
            DepartmentMissionsCountDto missionsCountDto = new DepartmentMissionsCountDto();

            // Sent Messions
            var missionsIQuerable = db.Missions.AsQueryable();

            missionsIQuerable = missionsIQuerable.Include(x => x.Sender).Where(x => x.Sender.DepartmentId == Convert.ToInt16(departmentId));
            missionsCountDto.SRunningCount = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == false).Count();
            missionsCountDto.SWaitingCount = missionsIQuerable.Where(x => x.TaskState == 0).Count();
            missionsCountDto.SCompletedCount = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == true && x.Is_Approved == false).Count();
            missionsCountDto.SApprovedCount = missionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == true && x.Is_Approved == true).Count();
            missionsCountDto.SRefusedCount = missionsIQuerable.Where(x => x.TaskState == 2).Count();
            
            // Received Missions
            var RmissionsIQuerable = db.Missions.AsQueryable();

            RmissionsIQuerable = RmissionsIQuerable.Include(x => x.Receiver).Where(x => x.Receiver.DepartmentId == Convert.ToInt16(departmentId));
            missionsCountDto.RRunningCount = RmissionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == false).Count();
            missionsCountDto.RWaitingCount = RmissionsIQuerable.Where(x => x.TaskState == 0).Count();
            missionsCountDto.RCompletedCount = RmissionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == true && x.Is_Approved == false).Count();
            missionsCountDto.RApprovedCount = RmissionsIQuerable.Where(x => x.TaskState == 1 && x.Is_Completed == true && x.Is_Approved == true).Count();
            missionsCountDto.RRefusedCount = RmissionsIQuerable.Where(x => x.TaskState == 2).Count();

            return missionsCountDto;
        }

        public Mission GetMission(int MissionId)
        {
            return db.Missions.Include(x => x.Sender).Include(x => x.Receiver).Include(x => x.Category).FirstOrDefault(a => a.Id == MissionId);
        }

        public bool IsMissionExists(int MissionId)
        {
            return db.Missions.Any(a => a.Id == MissionId);
        }

        //----
        public bool UpdateMission(Mission mission)
        {
            db.Missions.Update(mission);
            return save();
        }
        //----

        public bool save()
        {
            return db.SaveChanges() > 0 ? true : false;
        }

    }
}

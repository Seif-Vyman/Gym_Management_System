using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.CLasses
{
    public class AnalyticService : IAnalyticService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnalyticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticViewModel GetAnalyticData()
        {
            var sessions = _unitOfWork.SessionRepository.GetAll();
            return new AnalyticViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<Membership>().GetAll(X => X.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = sessions.Count(X => X.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(X => X.StartDate <= DateTime.Now && X.EndDate>= DateTime.Now),
                CompletedSessions = sessions.Count(X => X.EndDate < DateTime.Now)
            };
        }
    }
}

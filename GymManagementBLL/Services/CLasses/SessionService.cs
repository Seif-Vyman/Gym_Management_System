using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionsVeiwModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.CLasses
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerCategory();
            if (sessions.Any()) return [];

            //return sessions.Select(s => new SessionViewModel
            //{
            //    Id = s.Id,
            //    Description = s.Description,
            //    StartDate = s.StartDate,
            //    EndDate = s.EndDate,
            //    Capcity = s.Capacity,
            //    TrainerName = s.Trainer.Name,
            //    CategoryName = s.Category.CategoryName,
            //    AvailableSlots = s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(s.Id)
            //});

            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in MappedSessions)
                session.AvailableSlots = session.Capcity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);

            if (session is null) return null;
            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(sessionId);
            return mappedSession;
        }
        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try
            {
                if (!IsTrainerExists(CreatedSession.TrainerId)) return false;

                if (!IsCategoryExists(CreatedSession.CategoryId)) return false;

                if (!IsDateTimeValid(CreatedSession.StartDate, CreatedSession.EndDate)) return false;

                if (CreatedSession.Capacity > 25 || CreatedSession.Capacity < 1) return false;

                var mappedCreatedSession = _mapper.Map<Session>(CreatedSession);
                _unitOfWork.SessionRepository.Add(mappedCreatedSession);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Session Failed: {ex}");
                return false;
            }
        }
        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId); // why don't use getSessionWIthTrainerAndCategory?
            if (!IsSessionAvailableForUpdating(session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(session);

        }

        public bool UpdateSession(UpdateSessionViewModel UpdatedSession, int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdating(session!)) return false;
                if (!IsTrainerExists(UpdatedSession.TrainerId)) return false;
                if (!IsDateTimeValid(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false;

                _mapper.Map(UpdatedSession, session);
                session!.UpdatedAt = DateTime.Now;
                _unitOfWork.SessionRepository.Update(session);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed: {ex}");
                return false;

            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if(!IsSessionAvailableForRemoving(session!)) return false;

                _unitOfWork.SessionRepository.Delete(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed: {ex}");
                return false;
            }
        }

        #region Helper Methods

        private bool IsSessionAvailableForUpdating(Session session)
        {
            if (session is null) return false;
            if (session.EndDate < DateTime.Now || session.StartDate <= DateTime.Now) return false;
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;
            return true;
        }
        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session is null) return false;
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            if(session.StartDate > DateTime.Now) return false;
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;
            return true;
        }
        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }

        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        bool IsDateTimeValid(DateTime StartDate, DateTime EndDate) => StartDate < EndDate;

        


        #endregion
    }
}

using GymManagementBLL.ViewModels.SessionsVeiwModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel GetSessionById(int id);

        bool CreateSession(CreateSessionViewModel CreatedSession);

        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);

        bool UpdateSession(UpdateSessionViewModel UpdatedSession, int sessionId);
        bool RemoveSession(int sessionId);
    }
}

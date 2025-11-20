using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();

        TrainerViewModel? GetTrainerById(int id);

        bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel);

        TrainerToUpdateViewModel UpdateTrainerDetails(int id);
        bool UpdateTrainer(int id, TrainerToUpdateViewModel trainerToUpdateViewModel);

        bool DeleteTrainer(int id);

    }
}

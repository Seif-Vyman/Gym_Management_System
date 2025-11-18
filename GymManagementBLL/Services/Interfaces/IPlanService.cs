using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int planId);
        
        UpdatePlanViewModel GetPlanToUpdate(int planId);

        bool UpdatedPlan(int planId, UpdatePlanViewModel updatedModel);

        bool TogglePlanStatus(int planId);
    }
}

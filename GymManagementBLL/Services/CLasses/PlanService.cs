using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
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
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];

            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Duration = p.Duration,
                IsActive = p.IsActive
            });
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null) return null;
            return new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                Price = Plan.Price,
                Duration = Plan.Duration,
                IsActive = Plan.IsActive
            };
        }


        public UpdatePlanViewModel GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || plan.IsActive == false || IsActiveMembership(planId)) return null;

            return new UpdatePlanViewModel()
            {
                PlanName = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                Duration = plan.Duration
            };
        }

        public bool UpdatedPlan(int planId, UpdatePlanViewModel updatedModel)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || IsActiveMembership(planId)) return false;
            try
            {
                (plan.Description, plan.Price, plan.Duration, plan.UpdatedAt)
                    = (updatedModel.Description, updatedModel.Price, updatedModel.Duration, DateTime.Now);
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TogglePlanStatus(int planId)
        {
            var Repo = _unitOfWork.GetRepository<Plan>();
            var plan = Repo.GetById(planId);
            if (plan is null || IsActiveMembership(planId)) return false;
            //plan.IsActive = !plan.IsActive;
            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAt = DateTime.Now;
            try
            {
                Repo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods

        public bool IsActiveMembership(int planId)
        {
            var activeMemberships = _unitOfWork.GetRepository<Membership>().GetAll(m => m.PlanId == planId && m.Status == "Active");
            return activeMemberships.Any();
        }

        #endregion
    }
}

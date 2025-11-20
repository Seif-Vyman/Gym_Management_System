using AutoMapper;
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
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(plans);
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null) return null;
            
            return _mapper.Map<PlanViewModel>(Plan);
        }


        public UpdatePlanViewModel GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || plan.IsActive == false || IsActiveMembership(planId)) return null;

            
            return _mapper.Map<UpdatePlanViewModel>(plan);
        }

        public bool UpdatedPlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || IsActiveMembership(planId)) return false;
            try
            {
                //(plan.Description, plan.Price, plan.Duration, plan.UpdatedAt)
                //    = (updatedModel.Description, updatedModel.Price, updatedModel.Duration, DateTime.Now);
                _mapper.Map(updatedPlan, plan);
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

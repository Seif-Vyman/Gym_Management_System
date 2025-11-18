using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
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
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ICollection<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null) return [];
            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialty = t.Specialties.ToString()
            }).ToList();
        }

        public TrainerViewModel? GetTrainerById(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer is null) return null;
            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth,
                Specialty = trainer.Specialties.ToString(),
                Address = $"{trainer.Address.BuildingNumber}, {trainer.Address.Street}, {trainer.Address.City}",
            };
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        {
            if (IsValidEmail(createTrainerViewModel.Email) || IsValidPhone(createTrainerViewModel.Phone))
                return false;
            try
            {
                var trainer = new Trainer
                {
                    Name = createTrainerViewModel.Name,
                    Email = createTrainerViewModel.Email,
                    Phone = createTrainerViewModel.Phone,
                    DateOfBirth = createTrainerViewModel.DateOfBirth,
                    Gender = createTrainerViewModel.Gender,
                    Address = new Address
                    {
                        BuildingNumber = createTrainerViewModel.buildingNumber,
                        Street = createTrainerViewModel.Street,
                        City = createTrainerViewModel.City,
                    },
                    Specialties = createTrainerViewModel.Specialties,

                };
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public TrainerToUpdateViewModel UpdateTrainerDetails(int id)
        {
            var newTrainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (newTrainer is null) return null;
            return new TrainerToUpdateViewModel
            {
                Email = newTrainer.Email,
                Phone = newTrainer.Phone,
                Specialties = newTrainer.Specialties,
                buildingNumber = newTrainer.Address.BuildingNumber,
                Street = newTrainer.Address.Street,
                City = newTrainer.Address.City,
            };
        }
        public bool UpdateTrainer(int id, TrainerToUpdateViewModel trainerToUpdateViewModel)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if(trainer is null || IsValidEmail(trainer.Email) || IsValidPhone(trainer.Phone)) return false;
            try
            {
                trainer.Email = trainerToUpdateViewModel.Email;
                trainer.Phone = trainerToUpdateViewModel.Phone;
                trainer.Specialties = trainerToUpdateViewModel.Specialties;
                trainer.Address.BuildingNumber = trainerToUpdateViewModel.buildingNumber;
                trainer.Address.Street = trainerToUpdateViewModel.Street;
                trainer.Address.City = trainerToUpdateViewModel.City;
                trainer.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Trainer>().Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTrainer(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            var hasActiveSessions = _unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == id && s.StartDate > DateTime.Now).Any();
            if (trainer is null || hasActiveSessions) return false;
            try
            {
                _unitOfWork.GetRepository<Trainer>().Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region Helper Methods

        private bool IsValidEmail(string email) => _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        private bool IsValidPhone(string Phone) => _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == Phone).Any();



        #endregion
    }
}

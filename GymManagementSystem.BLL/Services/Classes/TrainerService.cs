using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Trainers;
using GymManagment.DAL;
using GymManagment.DAL.Models;
using GymManagment.DAL.Models.Enums;
using GymManagment.DAL.Repositories.Classes;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class TrainerService :ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Trainer> _repo;

        public TrainerService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
            _repo = _unitOfWork.GetRepository<Trainer>();
        }

        //get all
        public async Task<IEnumerable<TrainerViewModel>> GetTrainersAsync(CancellationToken ct)
        {
            var trainers = await _repo.GetAllAcync(ct:ct);
            var model = trainers.Select(trainers => new TrainerViewModel()
            {
                Id = trainers.Id,
                Name = trainers.Name,
                Phone = trainers.Phone,
                Email = trainers.Email,
                Specialties = trainers.Specialty.ToString(),
            });
            return model;
        }

        //trainer details
        public async Task<TrainerViewModel> TrainerDetailsAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _repo.GetByIdAcync(trainerId,ct);
            if (trainer == null) 
                return null;

            var model = new TrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Specialties = trainer.Specialty.ToString(),
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} Street - {trainer.Address.City}",
            };
            return model;
        }

        //create
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct)
        {
            var EmailCheck = await _repo.AnyAsync(t => t.Email == model.Email, ct);
            var PhoneCheck = await _repo.AnyAsync(t => t.Phone == model.Phone, ct);
            if (EmailCheck || PhoneCheck) 
                return false;
            var trainer = new Trainer()
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Specialty = model.Specialty,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street,
                },

            };
            _repo.Add(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }


        //update
        public async Task<UpdateTrainerViewModel> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _repo.GetByIdAcync(trainerId, ct);
            if (trainer == null)
                return null;

            var model = new UpdateTrainerViewModel()
            {
                Name = trainer.Name,
                DateOfBirth = trainer.DateOfBirth,
                Gender = trainer.Gender,
                Email = trainer.Email,
                Specialties = trainer.Specialty,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
            };
           return model;

        }

        public async Task<bool> UpdateTrainerAsync(int trainerId, UpdateTrainerViewModel model, CancellationToken ct)
        {
            var trainer =await _repo.GetByIdAcync(trainerId, ct);
            var EmailCheck = await _repo.AnyAsync(m => m.Email == model.Email && m.Id != trainerId, ct);
            var PhoneCheck = await _repo.AnyAsync(m => m.Phone == model.Phone && m.Id != trainerId, ct);

            if (EmailCheck || PhoneCheck)
                return false;
            trainer.Name = model.Name;
            trainer.DateOfBirth = model.DateOfBirth;
            trainer.Gender = model.Gender;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Specialty=model.Specialties;
            trainer.Address.BuildingNumber = trainer.Address.BuildingNumber;
            trainer.Address.Street = trainer.Address.Street;
            trainer.Address.City = trainer.Address.City;

            _repo.Update(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }



        //delete
        public async Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken ct)
        {
            var trainer = await _repo.GetByIdAcync(trainerId, ct);
            if(trainer == null) 
                return false;
            _repo.Delete(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }

       
    }
}

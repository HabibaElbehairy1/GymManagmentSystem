using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Sessions;
using GymManagment.DAL;
using GymManagment.DAL.Models;
using GymManagment.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Session> _repo;
        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repo = _unitOfWork.GetRepository<Session>();

        }
        //all
        public async Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory(ct);
            if (sessions is null) return null;

            var model = sessions.Select(session=>new SessionViewModel()
            {
                Id = session.Id,
                Description = session.Description,
                Capacity = session.Capacity,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                TrainerName=session.Trainer.Name,
                CategoryName=session.Category.Name,
            });
            foreach (var session in model) 
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }
            return model;
        }

        //details
        public async Task<SessionViewModel?> GetSessionDetailsAsync(int sessionId, CancellationToken ct)
        {
            var session = await _repo.GetByIdAcync(sessionId);
            if (session == null) return null;

            var model = new SessionViewModel()
            {
                Id = session.Id,
                Description = session.Description,
                Capacity = session.Capacity,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                TrainerName = session.Trainer.Name,
                CategoryName = session.Category.Name,
                AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct),
            };
            return model;
        }
        //create
        public async Task<bool> CreateSessionAsync(SessionViewModel model, CancellationToken ct)
        {
            var session = new Session() 
            { 
                Trainer = new Trainer()
                {
                    Name = model.TrainerName,
                },
                Category= new Category()
                {
                    Name = model.CategoryName, 
                },
                Description = model.Description,
                Capacity = model.Capacity,
                StartDate= model.StartDate,
                EndDate= model.EndDate,
            };
            _repo.Add(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        //update
        public async Task<SessionViewModel> ViewSessionToUpdateAsync(int sessionId, CancellationToken ct)
        {
            var session = await _repo.GetByIdAcync(sessionId);
            if (session is null)
                return null;
            var data = new SessionViewModel()
            {
                TrainerName =session.Trainer.Name,
                Description = session.Description,
                StartDate =session.StartDate,
                EndDate =session.EndDate,
            };

            return data;
        }

        public async Task<bool> UpdateSessionAsync(int sessionId, SessionViewModel model, CancellationToken ct)
        {
            var session = await _repo.GetByIdAcync(sessionId);

            session.Trainer.Name =model.TrainerName; 
            session.Description = model.Description;
            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;

            _repo.Update(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        //delete
        public async Task<bool> DeleteSessionAsync(int sessionId, CancellationToken ct)
        {
            var session = await _repo.GetByIdAcync(sessionId);
            if (session is null) return false;
            _repo.Delete(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

       
    }
}

using AutoMapper;
using AutoMapper.Execution;
using GymManagementSystem.BLL.Services.Interfaces;
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
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Session> _repo;
        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = _unitOfWork.GetRepository<Session>();

        }
        //all
        public async Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory(ct);
            if (sessions is null) return null;

            var result = _mapper.Map <IEnumerable<SessionViewModel>>(sessions);
            return result;
        }

        //details
        public async Task<SessionViewModel?> GetSessionDetailsAsync(int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId, ct);
            if (session == null) return null;

            var result = _mapper.Map<SessionViewModel>(session);
            return result;
        }
        //create
        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            var session = _mapper.Map<Session>(model);

            _repo.Add(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        //update
        public async Task<UpdateSessionViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct)
        {
            var session = await _repo.GetByIdAcync(sessionId);
            if (session is null)
                return null;

            var result = _mapper.Map<UpdateSessionViewModel>(session);

            return result;
        }

        public async Task<bool> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct)
        {
            var session = await _repo.GetByIdAcync(sessionId);

            if (model.EndDate <= model.StartDate)
                return false;

            _mapper.Map(model, session);
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


        public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<Category>().GetAllAcync(ct: ct);
        }
        public async Task<IEnumerable<Trainer>> GetTrainersAsync(CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<Trainer>().GetAllAcync(ct: ct);
        }


    }
}

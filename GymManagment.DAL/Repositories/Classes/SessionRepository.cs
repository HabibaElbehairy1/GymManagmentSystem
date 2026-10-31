using GymManagment.DAL.Models;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session> ,ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategory(CancellationToken ct = default)
        {
            return await _context.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).ToArrayAsync(ct);
        }
        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return await _context.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId);
        }
    }
}

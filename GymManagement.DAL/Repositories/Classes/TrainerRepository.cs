using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositories.Classes
{
    public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        private readonly GymDbContext _context;
        public TrainerRepository(GymDbContext context) : base(context)
        {

        }
    }
}

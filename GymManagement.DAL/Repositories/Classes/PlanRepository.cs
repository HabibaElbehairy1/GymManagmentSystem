using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository:GenericRepository<Plan>,IPlanRepository
    {
        public PlanRepository(GymDbContext context) : base(context) 
        { 
        }
    }
}

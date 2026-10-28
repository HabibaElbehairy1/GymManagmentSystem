using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.DbContexts;
using GymManagment.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositories.Classes
{
    public class PlanRepository:GenericRepository<Plan>,IPlanRepository
    {
        public PlanRepository(GymDbContext context) : base(context) 
        { 
        }
    }
}

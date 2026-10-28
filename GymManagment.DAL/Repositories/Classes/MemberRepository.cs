using GymManagment.DAL.Models;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Repositories.Classes
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly GymDbContext _context;

        public MemberRepository(GymDbContext context) : base(context)
        {
        }





    }

}

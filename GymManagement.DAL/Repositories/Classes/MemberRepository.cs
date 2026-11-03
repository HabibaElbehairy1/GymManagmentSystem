using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositories.Classes
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly GymDbContext _context;

        public MemberRepository(GymDbContext context) : base(context)
        {
        }





    }

}

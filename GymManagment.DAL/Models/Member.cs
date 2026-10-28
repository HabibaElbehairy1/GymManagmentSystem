using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        public HealthRecord HealthRecord { get; set; }
        public ICollection<MemberShip> Plans { get; set; }
        public ICollection<Booking> Sessions { get; set; }
    }
}

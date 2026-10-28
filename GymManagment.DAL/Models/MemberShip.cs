using GymManagment.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class MemberShip : BaseEntity
    {
        public Member Member { get; set; }
        public int MemberId { get; set; }
        public Plan Plan { get; set; }
        public int PlanId { get; set; }

        public DateTime EndDate { get; set; }
        public bool IsActive => Status == "Active";
        public string Status => EndDate > DateTime.UtcNow ? "Active" : "Expired" ;
    }
}

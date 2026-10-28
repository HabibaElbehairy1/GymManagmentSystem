using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class Booking : BaseEntity
    {
        public Member Member { get; set; }
        public int MemberId { get; set; }
        public Session Session { get; set; }
        public int SessionId { get; set; }
        public bool IsAttended { get; set; }

    }
}

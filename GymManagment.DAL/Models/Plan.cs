using GymManagment.DAL.Models;
using System.Collections;

namespace GymManagment.Models
{

    public class Plan : BaseEntity
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
        public int DurationDays { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MemberShip> Members { get; set; }

    }
}

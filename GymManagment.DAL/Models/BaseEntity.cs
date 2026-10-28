using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

    }
}

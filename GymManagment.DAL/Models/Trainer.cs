using GymManagment.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialty Specialty {  get; set; }
        public ICollection<Session> Session { get; set; }
    }
}

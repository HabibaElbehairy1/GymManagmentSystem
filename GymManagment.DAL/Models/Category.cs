using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class Category : BaseEntity
    {
        public string Name {  get; set; }
        public ICollection<Session> Session {  get; set; }
    }
}

using GymManagment.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.BLL.ViewModels.Members
{
    public class HealthRecordViewModel
    {
        [Range(0.1,300,ErrorMessage= "Height Must Be Greater Than") ]
        public decimal Height { get; set; }

        [Range(0.1, 300, ErrorMessage = "Weight Must Be Greater Than")]
        public decimal Weight { get; set; }

        [Required( ErrorMessage = "Blood Type Is Required")]
        public BloodType BloodType { get; set; } 

        public string? Note { get; set; }
    }
}

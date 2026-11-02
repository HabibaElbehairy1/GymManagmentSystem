using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.BLL.ViewModels.Sessions
{
    public class UpdateSessionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 5)]
        public string Description { get; set; } = null!;


        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please select a trainer")]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.BLL.ViewModels.Sessions
{
    public class CreateSessionViewModel
    {
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 5)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(1, 25, ErrorMessage = "Capacity must be between 1 and 25")]
        public int Capacity { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please select a trainer")]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}

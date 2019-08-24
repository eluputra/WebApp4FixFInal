using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyPaw.Models
{
    // Training dog model to hold information about the dog that was trained
    public class TrainingDogModel
    {
        // Primary key
        [Key]
        public int TrainingId { get; set; }

        public string DogName { get; set; }

        // Foreign key from the Trainers model
        [ForeignKey("TrainersModel")]
        public string TrainerId { get; set; }

        // Data type of Date
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Graduation Date")]
        public DateTime GraduationDate { get; set; }

        public string TrainingType { get; set; }

    }
}
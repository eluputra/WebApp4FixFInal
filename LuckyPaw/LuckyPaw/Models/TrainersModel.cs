using System;
using System.ComponentModel.DataAnnotations;

namespace LuckyPaw.Models
{
    // Trainers model to hold information about the dog trainer
    public class TrainersModel
    {
        // Primary key
        [Key]
        public string TrainerId { get; set; }

        public string TrainerName { get; set; }

        public string TrainerArea { get; set; }

        public int DogNumber { get; set; }

    }
}
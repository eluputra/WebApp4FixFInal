using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuckyPaw.Models
{   
    // Training services price model to hold information about a training service
    public class TrainingServicesPriceModel
    {
        // Primary key
        [Key]
        public int TrainingServicesPriceID { get; set; }

        public string TrainingName { get; set; }

        public double PriceTraining { get; set; }

        public string TrainingDesc { get; set; }

    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyPaw.Models
{
    // Cart Item Model to store bought puppies or trianing service
    public class CartItemModel
    {
        // Primary Key
        [Key]
        public int CartId { get; set; }

        // Foreign Key from pricing puppy model
        [ForeignKey("PricingPuppyModel")]
        public int PricePuppyID { get; set; }

        public string PricePuppyDesc { get; set; }

        public double PricePuppy { get; set; }

        // Foreign key from training services price model
        [ForeignKey("TrainingServicesPriceModel")]
        public int TrainingServicesPriceID { get; set; }

        public string TrainingName { get; set; }

        public double PriceTraining { get; set; }

        public int CartQty { get; set; }

        public string Email { get; set; }

    }
}
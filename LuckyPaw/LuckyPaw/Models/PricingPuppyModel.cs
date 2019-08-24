using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuckyPaw.Models
{
    // Price puppy model to hold puppy information and price
    public class PricingPuppyModel
    {
        // Primary key
        [Key]
        public int PricePuppyID { get; set; }

        public string PricePuppyDesc { get; set; }

        public double PricePuppy { get; set; }

        public string PricePuppyImageUrl { get; set; }

        public int PuppyQty { get; set; }

    }
}
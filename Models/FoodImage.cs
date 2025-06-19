using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Efood_Menu.Models
{
    public class FoodImage
    {
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        // Foreign key
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
    }
}
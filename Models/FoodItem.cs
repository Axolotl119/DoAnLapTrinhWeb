using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Efood_Menu.Models
{
    public class FoodItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Main image for the food item
        [StringLength(300)]
        public string ImageUrl { get; set; }

        // Navigation property for multiple images (additional images)
        public ICollection<FoodImage> Images { get; set; }

        // Foreign key
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
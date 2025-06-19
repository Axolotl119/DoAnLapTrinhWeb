using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Efood_Menu.Models
{
    public class Menu
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<FoodItem> FoodItems { get; set; }
        
    }
}
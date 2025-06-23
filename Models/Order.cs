using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebsiteBanHang_T4.Models;

namespace Efood_Menu.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Range(1000, 100000000)]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string? VoucherCode { get; set; }

        public decimal? DiscountApplied { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Done

        public ICollection<OrderItem>? OrderItems { get; set; }
    }


}

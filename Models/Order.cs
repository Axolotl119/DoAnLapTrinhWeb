using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Efood_Menu.Models
{
    public enum OrderType
    {
        DineIn,     // Ăn tại chỗ
        TakeAway,   // Mang đi
        Delivery    // Giao hàng
    }
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

        [Required]
        public OrderType OrderType { get; set; }

        public string? TableNumber { get; set; }
        public string ShippingAddress { get; set; }
		public string Notes { get; set; }
		public ICollection<OrderItem>? OrderItems { get; set; }
    }


}

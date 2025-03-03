using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace test_inventory.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; } 

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }


        [ForeignKey("customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }


        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }  // الكمية المباعة

        public DateTime SaleDate { get; set; }
    }
}

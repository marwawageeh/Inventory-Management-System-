using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace test_inventory.Models
{
    public class Purchas
    {
        [Key]
        public int Id { get; set; }  


        [ForeignKey("Product")]
        public int ProductId { get; set; } 
        public virtual Product Product { get; set; }  // Navigation Property


        [ForeignKey("supplier")]
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }  
        public DateTime PurchaseDate { get; set; }
    }
}

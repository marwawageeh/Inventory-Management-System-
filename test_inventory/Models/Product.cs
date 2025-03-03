using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace test_inventory.Models
{
    public class Product 
    {
        // ده المفتاح الأساسي للمنتج (Primary Key) وبيكون Unique لكل منتج 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }  // المفتاح الأساسي (Primary Key)

        [Required(ErrorMessage = "*")]
        [StringLength(50, MinimumLength = 1)]
        public string ProductName { get; set; }  // اسم المنتج

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }  // الكمية المتاحة من المنتج

        [ForeignKey("Category")]
        public int CategoryId { get; set; }  // المفتاح الخارجي للإشارة إلى الفئة

        public virtual Category Category { get; set; }

        // ده عشان نربط المنتج ده بمجموعة من المبيعات اللي تخصه
        public virtual ICollection<Sale> Sales { get; set; }  // مجموعة من المبيعات المرتبطة بالمنتج
        public virtual ICollection<Purchas> Purchases { get; set; }
    }
}

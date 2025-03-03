using System.ComponentModel.DataAnnotations;

namespace test_inventory.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }  // المفتاح الأساسي (Primary Key)

        [Required(ErrorMessage = "*")]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }  // اسم الفئة

        [StringLength(255)]
        public string Description { get; set; }  // وصف الفئة

        public virtual ICollection<Product> Product { get; set; }
    }
}

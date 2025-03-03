using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test_inventory.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; } 

        [Required(ErrorMessage = "*")]
        [StringLength(100)]
        public string Name { get; set; }  

        [StringLength(255)]
        public string Address { get; set; }  

        [StringLength(255)]
        public string ContactInfo { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }  

        public virtual ICollection<Sale> Sales { get; set; }  
    }
}

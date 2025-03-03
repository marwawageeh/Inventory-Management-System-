using Microsoft.AspNetCore.Identity;

namespace test_inventory.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }

    }
}

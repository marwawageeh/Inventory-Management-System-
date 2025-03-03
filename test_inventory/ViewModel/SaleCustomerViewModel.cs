using test_inventory.Models;

namespace test_inventory.ViewModel
{
    public class SaleCustomerViewModel
    {
            // بيانات البيع
            public int ProductId { get; set; }
            public int CustomerId { get; set; }
            public int Quantity { get; set; }
            public DateTime SaleDate { get; set; }

            // بيانات العميل
            public string CustomerName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }

            public string PaymentMethod { get; set; }

            // قوائم الخيارات
            public List<Product> Products { get; set; }
            public List<Customer> Customers { get; set; }
        

    }
}

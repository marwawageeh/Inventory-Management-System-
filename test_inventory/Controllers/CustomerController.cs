using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Text;
using test_inventory.Date;
using test_inventory.Models;

namespace test_inventory.Controllers
{
    public class CustomerController : Controller
    {
        Context db = new Context();
        public IActionResult Index()
        {
            var customers = db.customer.ToList();
            return View(customers);
        }
        // عرض فورم إنشاء عميل جديد
        
        public IActionResult ShowCreate()
        {
            return View();
        }

        // إضافة عميل جديد
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            db.customer.Add(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // عرض صفحة تأكيد حذف العميل
        public IActionResult Delete(int id)
        {
            var customer = db.customer.SingleOrDefault(c => c.Id == id);
            return View(customer);
        }

        // تنفيذ عملية الحذف بعد التأكيد
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = db.customer.SingleOrDefault(c => c.Id == id);
            db.customer.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // عرض صفحة تعديل العميل
        public IActionResult Edit(int id)
        {
            var customer = db.customer.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // تنفيذ عملية تعديل العميل
        [HttpPost]
        [ActionName("Edit")]
        public IActionResult EditConfirmed(Customer customer)
        {
            var existingCustomer = db.customer.SingleOrDefault(c => c.Id == customer.Id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.Name = customer.Name;
            existingCustomer.Address = customer.Address;
            existingCustomer.ContactInfo = customer.ContactInfo;
            existingCustomer.Phone = customer.Phone;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // تصدير بيانات العملاء كملف CSV
        public IActionResult ExportToCsv()
        {
            var customers = db.customer.ToList();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,Name,Address,ContactInfo,Phone");

            foreach (var customer in customers)
            {
                csvBuilder.AppendLine($"{customer.Id},{customer.Name},{customer.Address},{customer.ContactInfo},{customer.Phone}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return File(csvBytes, "text/csv", "Customers.csv");
        }

    }
}

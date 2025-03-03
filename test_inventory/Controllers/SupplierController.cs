using Microsoft.AspNetCore.Mvc;
using System.Text;
using test_inventory.Date;
using test_inventory.Models;

namespace test_inventory.Controllers
{
    public class SupplierController : Controller
    {
        Context db = new Context();

        public IActionResult Index()
        {
            var suppliers = db.supplier.ToList();
            return View(suppliers);
        }

        public IActionResult ShowCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Supplier supplier)
        {
            db.supplier.Add(supplier);
            db.SaveChanges();
            return RedirectToAction("Index"); 
        }

        public IActionResult Delete(int id)
        {
            var supplier = db.supplier.SingleOrDefault(a => a.Id == id); 
            return View(supplier);
        }

        
        [HttpPost, ActionName("Delete")] 
        public IActionResult DeleteConfirmed(int id)
        {
            var supplier = db.supplier.FirstOrDefault(a => a.Id == id);
            db.supplier.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public IActionResult Edit(int id)
        {
            var supplier = db.supplier.SingleOrDefault(a => a.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        
        [HttpPost]
        [ActionName("Edit")]
        public IActionResult EditConfirmed(Supplier supplier)
        {
            var existingSupplier = db.supplier.SingleOrDefault(a => a.Id == supplier.Id);
            if (existingSupplier == null)
            {
                return NotFound();
            }

            existingSupplier.Name = supplier.Name;
            existingSupplier.ContactInfo = supplier.ContactInfo;
            existingSupplier.Address = supplier.Address;
            existingSupplier.Phone = supplier.Phone;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public IActionResult ExportToCsv()
        {
            var suppliers = db.supplier.ToList(); 

            var csvBuilder = new StringBuilder(); 
            csvBuilder.AppendLine("Id,Name,ContactInfo,Address,phone");

            foreach (var supplier in suppliers)
            {
                csvBuilder.AppendLine($"{supplier.Id},{supplier.Name},{supplier.ContactInfo},{supplier.Address},{supplier.Phone}"); 
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString()); 

            return File(csvBytes, "text/csv", "Suppliers.csv"); 
        }


    }
}

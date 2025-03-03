using Microsoft.AspNetCore.Mvc;
using System.Text;
using test_inventory.Date;
using test_inventory.Models;

namespace test_inventory.Controllers
{
    public class PurchasController : Controller
    {
        Context db = new Context();

        public IActionResult Index()
        {
            var res = db.Purchas.ToList();
            return View(res);
        }

        public IActionResult Create()
        {
            ViewBag.pro = db.product.ToList();
            ViewBag.sup = db.supplier.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Purchas pur)
        {
            if (ModelState.IsValid)
            {// العثور على المنتج المراد إضافة الكمية إليه
                var product = db.product.SingleOrDefault(p => p.Id == pur.ProductId);
                var supplier = db.supplier.SingleOrDefault(p => p.Id == pur.SupplierId);
                if (product == null)
                {
                    ModelState.AddModelError("", "the product is not avaliable");
                    ViewBag.pro = db.product.ToList();
                    return View(pur);
                }
                if (supplier == null)
                {
                    ModelState.AddModelError("", "the supplier is not avaliable");
                    ViewBag.sup = db.supplier.ToList();
                    return View(pur);
                }
                // إضافة الكمية المشتراة إلى الكمية المتاحة
                product.Quantity += pur.Quantity;
                // إضافة سجل الشراء إلى قاعدة البيانات
                db.Purchas.Add(pur);
                // حفظ التغييرات
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.pro = db.product.ToList();
                ViewBag.sup = db.supplier.ToList();
                return View(pur);
            }
        }


        public IActionResult Delete(int id)
        {
            var pur = db.Purchas.SingleOrDefault(a => a.Id == id);
            return View(pur);
        }

        [HttpPost, ActionName("delete")]
        public IActionResult Deleteconfirmed(int id)
        {
            var purchas = db.Purchas.FirstOrDefault(a => a.Id == id);
            db.Purchas.Remove(purchas);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public IActionResult Edit(int id)
        {

            var pur = db.Purchas.SingleOrDefault(a => a.Id == id);
            if (pur == null)
            {
                return NotFound();
            }

            ViewBag.pro = db.product.ToList();

            return View(pur);
        }

        [HttpPost]
        [ActionName("Edit")]

        public IActionResult Editconfirmed(Purchas pur)
        {
            var existingPurchas = db.Purchas.SingleOrDefault(a => a.Id == pur.Id);
            if (!ModelState.IsValid)
            {
                // إذا لم يكن النموذج صالحًا، أعد تحميل قائمة المنتجات
                ViewBag.pro = db.product.ToList();
                return View("Edit", pur); // إرجاع العرض مع الأخطاء
            }
            if (existingPurchas == null)
            {
                return NotFound();
            }
            existingPurchas.PurchaseDate = pur.PurchaseDate;
            existingPurchas.ProductId = pur.ProductId;
            existingPurchas.Quantity = pur.Quantity;


            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ExportToCsv()
        {
            var purchas = db.Purchas.ToList();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,Purchasdate,Quantity,productid");

            foreach (var pur in purchas)
            {
                csvBuilder.AppendLine($"{pur.Id},{pur.PurchaseDate},{pur.Quantity},{pur.ProductId}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            return File(csvBytes, "text/csv", "Purchas.csv");
        }

    }
}

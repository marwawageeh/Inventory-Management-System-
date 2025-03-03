using Microsoft.AspNetCore.Mvc;
using System.Text;
using test_inventory.Date;
using test_inventory.Models;
using System.IO;
using System.Text;

namespace test_inventory.Controllers
{
    public class ProductController : Controller
    {
        // بننشئ كائن من سياق قاعدة البيانات عشان نقدر نتعامل مع قاعدة البيانات من خلاله
        Context db = new Context();

        // دالة بتجيب كل المنتجات من قاعدة البيانات وتعرضهم في الصفحة
        public IActionResult Index()
        {
            var res = db.product.ToList();
            return View(res);
        }



        // دالة بتعرض فورم إنشاء منتج جديد
        public IActionResult ShowCreate()
        {
            ViewBag.Category = db.category.ToList();
            return View();
        }

        // دالة بتستقبل بيانات المنتج الجديد من الفورم، وتضيفه لقاعدة البيانات
        [HttpPost]
        public IActionResult Create (Product pro)
        {
            if (ModelState.IsValid)
            {
                if (pro.Quantity < 0)
                {
                    ModelState.AddModelError("", "the amount can not be negative ");
                    ViewBag.Category = db.category.ToList();  // إعادة تحميل الفئات في حال حدوث خطأ
                    return View(pro);
                }

                // إضافة المنتج إلى قاعدة البيانات
                db.product.Add(pro);
                db.SaveChanges();

                // إعادة التوجيه إلى الصفحة الرئيسية
                return RedirectToAction("Index");
            }

            // في حال كانت البيانات غير صحيحة، إعادة تحميل الفئات وعرض النموذج مرة أخرى
            ViewBag.Category = db.category.ToList();
            return View(pro);

        }

        public IActionResult Delete(int id)
        {
            var dept = db.product.SingleOrDefault(a => a.Id == id);
            return View(dept);
        }

        // دالة بتنفذ عملية الحذف فعلياً بعد التأكيد
        [HttpPost, ActionName("delete")]//فده معناها إنها مش هتشتغل إلا لما يتبعت لها طلب POST
        public IActionResult Deleteconfirmed(int id)
        {
            var pro = db.product.FirstOrDefault(a => a.Id == id);
            db.product.Remove(pro);
            db.SaveChanges();
            return RedirectToAction("Index");

        }



        // دالة بتعرض فورم تعديل منتج معين
        public IActionResult Edit(int id)
        {
            var crs = db.product.SingleOrDefault(a => a.Id == id);
            if (crs == null)
            {
                return NotFound();
            }
            return View(crs);
        }

        // دالة بتنفذ عملية التعديل على المنتج بعد التأكيد
        [HttpPost]
        [ActionName("Edit")]
        public IActionResult Editconfirmed(Product product)
        {
            var existingProduct = db.product.SingleOrDefault(a => a.Id == product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.Quantity = product.Quantity;

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // دالة بتصدر البيانات الخاصة بالمنتجات كملف CSV
        public IActionResult ExportToCsv()
        {
            var products = db.product.ToList();// بنجيب كل المنتجات

            var csvBuilder = new StringBuilder();// بنضيف عنوان الأعمدة في أول سطر
            csvBuilder.AppendLine("Id,ProductName,Quantity");

            foreach (var product in products)
            {
                csvBuilder.AppendLine($"{product.Id},{product.ProductName},{product.Quantity}");// بنضيف بيانات كل منتج في سطر
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());// بنحول البيانات لبايتات عشان نعمل ملف CSV


            return File(csvBytes, "text/csv", "Products.csv");// بنرجع الملف للتحميل
        }
        

        

    }
}

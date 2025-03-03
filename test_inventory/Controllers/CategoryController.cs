using Microsoft.AspNetCore.Mvc;
using System.Text;
using test_inventory.Date;
using test_inventory.Models;

namespace test_inventory.Controllers
{
    public class CategoryController : Controller
    {
        Context db = new Context();
        public IActionResult Index()
        {
            var categories = db.category.ToList();
            return View(categories);
        }
        // دالة بتعرض فورم إنشاء تصنيف جديد
        public IActionResult ShowCreate()
        {
            return View();
        }

        // دالة بتستقبل بيانات التصنيف الجديد من الفورم، وتضيفه لقاعدة البيانات
        [HttpPost]
        public IActionResult Create(Category cat)
        {
            db.category.Add(cat);
            db.SaveChanges();
            return RedirectToAction("Index"); // بنرجع للصفحة الرئيسية بعد الإضافة
        }

        // دالة بتعرض صفحة تأكيد حذف تصنيف معين
        public IActionResult Delete(int id)
        {
            var category = db.category.SingleOrDefault(c => c.Id == id); // بنجيب التصنيف من قاعدة البيانات باستخدام الـ id
            return View(category);
        }

        // دالة بتنفذ عملية الحذف فعلياً بعد التأكيد
        [HttpPost, ActionName("Delete")] // معناها إنها مش هتشتغل إلا لما يتبعت لها طلب POST
        public IActionResult DeleteConfirmed(int id)
        {
            var category = db.category.FirstOrDefault(c => c.Id == id);
            db.category.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // دالة بتعرض فورم تعديل تصنيف معين
        public IActionResult Edit(int id)
        {
            var category = db.category.SingleOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // دالة بتنفذ عملية التعديل على التصنيف بعد التأكيد
        [HttpPost]
        [ActionName("Edit")]
        public IActionResult EditConfirmed(Category category)
        {
            var existingCategory = db.category.SingleOrDefault(c => c.Id == category.Id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // دالة بتصدر البيانات الخاصة بالتصنيفات كملف CSV
        public IActionResult ExportToCsv()
        {
            var categories = db.category.ToList(); // بنجيب كل التصنيفات

            var csvBuilder = new StringBuilder(); // بنضيف عنوان الأعمدة في أول سطر
            csvBuilder.AppendLine("Id,CategoryName,Description");

            foreach (var category in categories)
            {
                csvBuilder.AppendLine($"{category.Id},{category.Name},{category.Description}"); // بنضيف بيانات كل تصنيف في سطر
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString()); // بنحول البيانات لبايتات عشان نعمل ملف CSV

            return File(csvBytes, "text/csv", "Categories.csv"); // بنرجع الملف للتحميل
        }



    }
}

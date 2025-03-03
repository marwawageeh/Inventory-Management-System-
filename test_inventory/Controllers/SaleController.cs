using Microsoft.AspNetCore.Mvc;
using System.Text;
using test_inventory.Date;
using test_inventory.Models;
using test_inventory.ViewModel;

namespace test_inventory.Controllers
{
    public class SaleController : Controller
    {
        Context db = new Context();

        public IActionResult Index()
        {
            var res = db.sale.ToList();
            return View(res);
        }

        public IActionResult Create()
        {
            ViewBag.pro = db.product.ToList();
            ViewBag.cus = db.customer.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Sale sal)
        {
            if (ModelState.IsValid)
            { // البحث عن المنتج للتحقق من الكمية المتاحة
                var product = db.product.SingleOrDefault(p => p.Id == sal.ProductId);
                var customer = db.customer.SingleOrDefault(p => p.Id == sal.CustomerId);

                if (product == null)
                {
                    ModelState.AddModelError("", "the product is not avaliable");
                    ViewBag.pro = db.product.ToList();
                    return View(sal);
                }

                if (customer == null)
                {
                    ModelState.AddModelError("", "the customer is not avaliable");
                    ViewBag.pro = db.product.ToList();
                    ViewBag.cus = db.customer.ToList();  // إعادة تحميل العملاء
                    return View(sal);
                }

                if (product.Quantity >= sal.Quantity)
                {
                    // تقليل الكمية المتاحة من المنتج
                    product.Quantity -= sal.Quantity;

                    // إضافة السجل إلى جدول المبيعات
                    db.sale.Add(sal);

                    // حفظ التغييرات
                    db.SaveChanges();

                    return RedirectToAction("Credit");
                }
                else
                {
                    // رسالة خطأ إذا كانت الكمية المطلوبة غير متاحة
                    ModelState.AddModelError("", "not avilable");
                }
            }
            
                ViewBag.pro = db.product.ToList();
                ViewBag.cus = db.customer.ToList();
            return View(sal);

        }


        public IActionResult Delete(int id)
        {
            var sal = db.sale.SingleOrDefault(a => a.Id == id);
            return View(sal);
        }

        [HttpPost, ActionName("delete")]
        public IActionResult Deleteconfirmed(int id)
        {
            var sale = db.sale.FirstOrDefault(a => a.Id == id);
            db.sale.Remove(sale);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public IActionResult Edit(int id)
        {

            var sal = db.sale.SingleOrDefault(a => a.Id == id);
            if (sal == null)
            {
                return NotFound();
            }

            ViewBag.pro = db.product.ToList();

            return View(sal);
        }

        [HttpPost]
        [ActionName("Edit")]

        public IActionResult Editconfirmed(Sale sal)
        {
            var existingsale = db.sale.SingleOrDefault(a => a.Id == sal.Id);
            if (!ModelState.IsValid)
            {
                // إذا لم يكن النموذج صالحًا، أعد تحميل قائمة المنتجات
                ViewBag.pro = db.product.ToList();
                return View("Edit", sal); // إرجاع العرض مع الأخطاء
            }
            if (existingsale == null)
            {
                return NotFound();
            }
            existingsale.SaleDate = sal.SaleDate;
            existingsale.ProductId = sal.ProductId;
            existingsale.Quantity = sal.Quantity;


            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult ExportToCsv()
        {
            var sale = db.sale.ToList();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,saledate,Quantity,productid");

            foreach (var sal in sale)
            {
                csvBuilder.AppendLine($"{sal.Id},{sal.SaleDate},{sal.Quantity},{sal.ProductId}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            return File(csvBytes, "text/csv", "sale.csv");
        }


        public IActionResult CreateSaleWithCustomer()
        {
            var model = new SaleCustomerViewModel
            {
                Products = db.product.ToList(),
                Customers = db.customer.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateSaleWithCustomer(SaleCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                // التحقق من المنتج
                var product = db.product.SingleOrDefault(p => p.Id == model.ProductId);
                if (product == null || product.Quantity < model.Quantity)
                {
                    ModelState.AddModelError("", "Product is not available or insufficient quantity.");
                    model.Products = db.product.ToList();
                    model.Customers = db.customer.ToList();
                    return View(model);
                }

                int customerId = model.CustomerId;
                if (customerId == 0 && !string.IsNullOrEmpty(model.CustomerName))
                {
                    // إضافة عميل جديد
                    var newCustomer = new Customer
                    {
                        Name = model.CustomerName,
                        ContactInfo = model.Email,
                        Phone = model.Phone
                    };

                    db.customer.Add(newCustomer);
                    db.SaveChanges();
                    customerId = newCustomer.Id; // الحصول على معرف العميل الجديد
                }

                // إنشاء عملية البيع
                var sale = new Sale
                {
                    ProductId = model.ProductId,
                    CustomerId = customerId, // استخدام المعرف المحدد أو الجديد
                    Quantity = model.Quantity,
                    SaleDate = model.SaleDate
                };

                // تحديث كميات المنتج
                product.Quantity -= model.Quantity;

                // حفظ بيانات البيع
                db.sale.Add(sale);
                db.SaveChanges();

                // التحقق من طريقة الدفع
                if (model.PaymentMethod == "Credit")
                {
                    // إعادة التوجيه إلى صفحة الدفع بالبطاقة الائتمانية
                    return RedirectToAction("Credit");
                }

                // إذا كانت طريقة الدفع نقدًا، التوجيه إلى Index
                return RedirectToAction("Index");
            }

            model.Quantity = model.Quantity <= 0 ? 0 : model.Quantity;
            model.SaleDate = model.SaleDate == DateTime.MinValue ? DateTime.Now : model.SaleDate;

            // إعادة تحميل البيانات إذا كان هناك خطأ
            model.Products = db.product.ToList();
            model.Customers = db.customer.ToList();
            return View(model);
        }

        public IActionResult Credit()
        {
            return View(); // This will look for Views/Sale/Credit.cshtml
        }
        public IActionResult Recipt(SaleCustomerViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Index"); // Or another appropriate action if the model is null.
            }
            return View(model);
        }



    }
}

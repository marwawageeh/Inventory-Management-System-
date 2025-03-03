using System.Collections.Generic;
using test_inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace test_inventory.Date
{
    // هنا بنعرّف كلاس خاص بسياق قاعدة البيانات وبيورث من DbContext (ده الكلاس اللي بيخلينا نتعامل مع قاعدة البيانات)
    public class Context : IdentityDbContext<ApplicationUser>
    {
        // بنعمل خاصية من نوع DbSet مخصوصة بجدول المنتجات، وده بيمثل الـ Product اللي في قاعدة البيانات
        public DbSet<Product> product { get; set; }

        // بنعمل خاصية من نوع DbSet مخصوصة بجدول المشتريات، وده بيمثل الـ Purchas اللي في قاعدة البيانات
        public DbSet<Purchas> Purchas { get; set; }
        public DbSet<Sale> sale { get; set; }
        public DbSet<Supplier> supplier { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<Category> category { get; set; }


        // هنا بنعمل إعدادات الاتصال بقاعدة البيانات باستخدام SQL Server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // بنحدد إعدادات الاتصال بقاعدة البيانات (اسم السيرفر واسم الداتا بيز وغيرها)
            optionsBuilder.UseSqlServer("Server=MARWA\\SQLEXPRESS ; Database=Te_inventory ;Integrated Security = true ;Trust Server Certificate=true");

            // استدعاء الدالة الأصلية عشان أي إعدادات إضافية موجودة في الكلاس الأساسي
            base.OnConfiguring(optionsBuilder);
        }
    }
}

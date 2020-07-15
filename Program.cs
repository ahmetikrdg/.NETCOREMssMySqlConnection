using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFD
{


    public class ShopContext : DbContext
    {
        public DbSet<Product> products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });//entity komutum sqlde nasıl göremk için ekledim
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory).UseMySql(@"server=localhost;port=3306;database=ShopDb;user=root;password=1532blmz");

           // //entity komutum sqlde nasıl göremk için ekledim bu satırı
        }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Addproducts();
        }

        static void DeleteProduct(int id)
        {

            using (var db = new ShopContext())
            {
                var p =new Product(){Id=6};
                db.Entry(p).State=EntityState.Deleted;
                db.SaveChanges();
                //db entry ve silmek istediğim obje (p) objesi
                //ve bunun stati = entity.state aracılığı ile delete
            }
           /* ------------
            using (var db = new ShopContext())
            {
                var p = db.products.FirstOrDefault(i => i.Id == id);
                if (p != null)
                {
                    db.products.Remove(p);
                    db.SaveChanges();
                    Console.WriteLine("Veri Silindi");
                }
            }*/
        }
        static void UpdateProduct()
        {
            using (var db = new ShopContext())
            {
                var p = db.products.Where(i => i.Id == 1).FirstOrDefault();
                if (p != null)
                {
                    p.Price = 2500;
                    db.products.Update(p);//tekbir ürün veya updaterange ile liste güncelleyebilirsin
                    db.SaveChanges();
                }//buranın dezavantajı sadece fiyatı güncelliyosun ama diğer alanlarıda güncelliyo alttaki yaklaşım dah doğru
            }


            /*
                        ----------------------
                        using (var db = new ShopContext())
                        {
                            var entity = new Product() { Id = 1 };
                            db.products.Attach(entity);
                            entity.Price = 3000;
                            db.SaveChanges();
                        }
                        ---------------------
                        change tracking = alınan objenin takibi yapılır obje üzerinde değişiklik olursa db.savechange aracılııyla vt dede yapılır.
                        using (var db = new ShopContext())
                        {
                            var p = db.products.Where(i => i.Id == 1).FirstOrDefault();
                            if (p != null)
                            {//p boş değilse
                                p.Price *= 1.2m;
                                db.SaveChanges();
                                Console.WriteLine("Güncelleme Yapıldı");
                            }
                        } */
        }
        static void GetProductByName(string name)

        {

            using (var context = new ShopContext())
            {
                var result = context.products.Where(p => p.Name.Contains(name))//cona parametre gönderdim ve ilgili parametre name içinde varsa bize true döndürür o p değeride resulta gider
                .ToList();
                foreach (var p in result)
                {
                    Console.WriteLine(p.Id + " " + p.Name + " " + p.Price);
                }

            }
        }
        static void GetProductById(int id)
        {

            using (var context = new ShopContext())
            {
                var Result = context.products
                .Where(p => p.Id == id)
                .FirstOrDefault();

                Console.WriteLine(Result.Name + " " + Result.Price);
            }
        }
        static void Addproducts()
        {

            using (var db = new ShopContext())//using içine aldımki işim bitince bellekten gitsin
            {
                var products = new List<Product>()
                {
                    new Product { Name = "İphone", Price = 4000 },
                    new Product { Name = "Xiomi", Price = 3500 }
                };

                foreach (var p in products)
                {
                    db.products.Add(p);
                }
                db.SaveChanges();
                // db.products.AddRange(products); bu foreachın yaptığı işlemi yapar
                Console.WriteLine("Kayıt Eklendi1111111111");
            }


        }
        static void Addproduct()
        {

            using (var db = new ShopContext())
            {
                var p = new Product { Name = "Samsung", Price = 3000 };
                db.products.Add(p);//benim tanımlamış olduğum nesne entityfream tarafında bir entity olarak tanımlanıyor 
                db.SaveChanges();//bekelyen değişiklikler databaseye aktarılır
                Console.WriteLine("Kayıt Eklendi2");
            }


        }
        static void GetAllProducts()
        {

            using (var context = new ShopContext())
            {
                var product = context.products.Select(p => new { p.Name, p.Price }).ToList();

                foreach (var p in product)
                {
                    Console.WriteLine(p.Name + " " + p.Price);
                }
            }

        }

    }
}
//mysql mssgl için ne yaptık miggrationu sildik MySQL provider for Entity Framework Core nuget paketini yükledik ayrı ayrı
//Create the database işlemi için "dotnet ef migrations add InitialCreate / dotnet ef database update"ları yazdık.
//Vtde tablolar oluştu sonra bilgilerin kayıt olması için mainde addproductu çalıştırdım.
//yani hangi sql türüyle çalıştığımız önemli değil önemli contextte hangi veri tabanı ile ilişkilendiyirosun.
//ve ilişkilendirdiğin server tipinin provediri projenizde yüklümü yüklü değilmi bunu yapınca geri kalan
//hiçbi işlem değişmez crud falan ellemiyosun onu entity yapıyor konuşuyor onunla.








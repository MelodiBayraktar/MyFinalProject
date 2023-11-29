using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework;
//db tabloları ile proje classlarını bağlar
public class NortwindContext:DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=NORTHWND;User Id=SA;Password=reallyStrongPwd123;Trusted_Connection=false;TrustServerCertificate=True;MultiSubnetFailover=True");
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
}
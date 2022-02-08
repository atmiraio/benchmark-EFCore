using Microsoft.EntityFrameworkCore;

namespace EFCore6
{
    public class EFCore6DbContext : DbContext
    {
        public EFCore6DbContext(DbContextOptions<EFCore6DbContext> options) : base(options) { }

        public DbSet<SimpleEntity> SimpleEntities { get; set; }
        public DbSet<Entity> Entities { get; set; }

        public DbSet<Father> Fathers { get; set; }
        public DbSet<Child> Children { get; set; }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        //    optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=EFCore6;Trusted_Connection=True;")
        //    .EnableThreadSafetyChecks(false);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Father>()
                .HasOne(f => f.Child)
                .WithOne(c => c.Father)
                .HasForeignKey<Child>(c => c.ChildOfFatherId);

            modelBuilder.Entity<Grade>()
                .HasMany(g => g.Students)
                .WithOne(s => s.Grade)
                .HasForeignKey(s => s.CurrentGradeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierProduct>().HasKey(sp => new { sp.SupplierId, sp.ProductId });

            modelBuilder.Entity<SupplierProduct>()
                .HasOne<Product>(sp => sp.Product)
                .WithMany(p => p.SupplierProduct)
                .HasForeignKey(sp => sp.ProductId);

            modelBuilder.Entity<SupplierProduct>()
                .HasOne<Supplier>(sp => sp.Supplier)
                .WithMany(p => p.SupplierProduct)
                .HasForeignKey(sp => sp.SupplierId);
        }
    }

    public class SimpleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Property1 { get; set; }
        public double Property2 { get; set; }
        public decimal Property3 { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }

    // One-to-One Relationships
    public class Father
    {
        public int FatherId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Child Child { get; set; }
    }

    public class Child
    {
        public int ChildId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ChildOfFatherId { get; set; }
        public Father Father { get; set; }
    }

    // One-to-Many Relationships
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CurrentGradeId { get; set; }
        public Grade Grade { get; set; }
    }

    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string Section { get; set; }

        public ICollection<Student> Students { get; set; }
    }

    // Many-to-Many Relationships
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        public IList<SupplierProduct> SupplierProduct { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public IList<SupplierProduct> SupplierProduct { get; set; }
    }

    public class SupplierProduct
    {
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Accounting_for_purchases
{
    public class ConnectDB : DbContext
    {
        public DbSet<Model.Sprav> Sprav { get; set; }
        public DbSet<Model.Product> Product { get; set; }
        public DbSet<Model.Order> Order { get; set; }
        public ConnectDB()
        {
             //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();




        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("Data Source=Base.db");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model.Sprav>(b => b.ToTable("sprav"));
            modelBuilder.Entity<Model.Product>(b => b.ToTable("product"));
            modelBuilder.Entity<Model.Order>(b => b.ToTable("order"));
        }
    }
}

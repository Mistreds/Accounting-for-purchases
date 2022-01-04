using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Accounting_for_purchases
{
    public class ConnectDB : DbContext
    {
        public DbSet<Model.Sprav> Sprav { get; set; }
        public DbSet<Model.Product> Product { get; set; }
        public DbSet<Model.Order> Order { get; set; }
        public DbSet<Model.Employee> Employee { get; set; }
        public DbSet<Model.Store> Store { get; set; }
        public ConnectDB()
        {
            
            //Database.EnsureDeleted();
            App.is_connect = true;
             try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                App.is_connect=false;
            }
            //Database.Migrate();




        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

           optionsBuilder.UseMySql(Setting.GetDatabaseSetting() );

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model.Sprav>(b => b.ToTable("sprav"));
            modelBuilder.Entity<Model.Product>(b => b.ToTable("product"));
            modelBuilder.Entity<Model.Order>(b => b.ToTable("order"));
            modelBuilder.Entity<Model.Employee>(b => b.ToTable("employee"));
            modelBuilder.Entity<Model.Store>(b => b.ToTable("store"));
            modelBuilder.Entity<Model.Store>().HasData(new Model.Store[] { new Model.Store { Id = 1, Address = "", Name = "Пусто" } });
            modelBuilder.Entity<Model.Employee>().HasData(new Model.Employee[] { new Model.Employee { Fio = "Admin", Id = 1,IsAdmin = true, Login = "Admin", Password = "Admin", StoreId = 1 } }) ;
            
            modelBuilder.Entity<Model.Product>().HasKey(p => p.Id);
        }
    }
}

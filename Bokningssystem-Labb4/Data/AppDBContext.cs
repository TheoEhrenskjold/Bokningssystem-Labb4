using Microsoft.EntityFrameworkCore;
using BokningsModels;
using System;

namespace Bokningssystem_Labb4.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<AppHistory> Apphistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 1,
                Name = "Ebba",
                EMail = "Ebba@gmail.com"
            });
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                CustomerId = 2,
                Name = "Anders",
                EMail = "Anders@gmail.com"
            });
            modelBuilder.Entity<Company>().HasData(new Company
            {
                CompanyID = 1,
                CompanyName = "Grassfish",
                Email = "Grass@fish.com"
            });
            modelBuilder.Entity<Company>().HasData(new Company
            {
                CompanyID = 2,
                CompanyName = "Systeminstallation",
                Email = "System@installation.com"
            });
        }
        }
    }

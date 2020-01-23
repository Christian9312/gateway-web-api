using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Gateways.Domain.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Gateway> Gateways { get; set; }

        public DbSet<Peripheral> Peripherals { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Gateway>().ToTable("Gateways");
            builder.Entity<Gateway>().HasKey(p => p.SerialNumber);
            builder.Entity<Gateway>().Property(p => p.SerialNumber).IsRequired();
            builder.Entity<Gateway>().Property(p => p.Name).HasMaxLength(50);
            builder.Entity<Gateway>().Property(p => p.Address).IsRequired();
            builder.Entity<Gateway>().HasMany(p => p.AssociatedPeripherals)
                .WithOne(p => p.Gateway)
                .HasForeignKey(p => p.GatewayId);

            

            builder.Entity<Peripheral>().ToTable("Peripherals");
            builder.Entity<Peripheral>().HasKey(p => p.UId);
            builder.Entity<Peripheral>().Property(p => p.UId).IsRequired();
            builder.Entity<Peripheral>().Property(p => p.Vendor).HasMaxLength(50);
            builder.Entity<Peripheral>().Property(p => p.CreationDate).IsRequired();
            builder.Entity<Peripheral>().Property(p => p.Status).IsRequired();

            builder.Entity<Gateway>().HasData
            (
                new Gateway { SerialNumber = "213213213", Name = "Bad gateway", Address = "192.168.1.1" , AssociatedPeripherals = new List<Peripheral>()},
                new Gateway { SerialNumber = "232323243", Name = "Good gateway", Address = "192.168.1.2", AssociatedPeripherals = new List<Peripheral>()}
            );

            builder.Entity<Peripheral>().HasData
            (
                new Peripheral() { UId = uint.MaxValue, CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Offline, Vendor =  "ME", GatewayId = "213213213"},
                new Peripheral() { UId = uint.MaxValue-1 , CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Online, Vendor =  "You", GatewayId = "232323243"}
            );
        }
    }
}

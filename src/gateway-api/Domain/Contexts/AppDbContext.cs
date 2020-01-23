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

            var guid = Guid.NewGuid().ToString();
            builder.Entity<Gateway>().HasData
            (
                new Gateway { SerialNumber = Guid.NewGuid().ToString(), Name = "Bad gateway", Address = "192.168.1.1" , AssociatedPeripherals = new List<Peripheral>()},
                new Gateway { SerialNumber = guid, Name = "Good gateway", Address = "192.168.1.2", AssociatedPeripherals = new List<Peripheral>()}
            );

            builder.Entity<Peripheral>().HasData
            (
                new Peripheral() { UId = uint.MaxValue, CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Offline, Vendor =  "ME", GatewayId = guid},
                new Peripheral() { UId = uint.MaxValue-1 , CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Online, Vendor =  "You", GatewayId = guid}
            );
        }
    }
}

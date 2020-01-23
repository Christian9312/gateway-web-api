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

            DataSeedGeneration(builder, 50);

        }

        private void DataSeedGeneration(ModelBuilder builder, int quantityOfResources)
        {

            var gatewayIds = new int [quantityOfResources].Select(item => Guid.NewGuid().ToString()).ToArray();

            var gateways = new int [quantityOfResources]
                .Select((item, i) =>
                    new Gateway
            {
                SerialNumber = gatewayIds[i],
                Address = $"192.168.1.{new Random().Next(0, 255)}",
                Name = $"Gateway {i}"
            }).ToList();


            var peripheralIds = new int [quantityOfResources].Select(item => (uint) new Random().Next(0, maxValue:int.MaxValue)).ToArray();

            var peripherals = new int [quantityOfResources].Select((item, i) => new Peripheral
            {
                UId = peripheralIds[i],
                Status = new Random().Next(0, 2) == 0 ? PeripheralStatus.Offline : PeripheralStatus.Online,
                CreationDate = DateTimeOffset.UtcNow,
                Vendor = $"Vendor {i}",
                GatewayId = gatewayIds[new Random().Next(0,gatewayIds.Length)]
            });

            builder.Entity<Gateway>().HasData(gateways);
            builder.Entity<Peripheral>().HasData(peripherals);
        }
    }
}

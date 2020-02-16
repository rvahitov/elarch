#nullable enable
using ElArch.Storage.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace ElArch.Storage
{
    public class ElArchStorageContext : DbContext
    {
        public ElArchStorageContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FieldReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new IntegerFieldReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new DecimalFieldReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new DateTimeFieldReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentTypeReadModelConfiguration());
        }
    }
}
using ElArch.Storage.DocumentType.ReadModels;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ElArch.Storage
{
    public class ElArchContext : DbContext
    {
        public ElArchContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentTypeReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new FieldReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new IntegerFieldReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new DecimalFieldReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new DateTimeFieldReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentViewReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new ViewFieldReadModelConfiguration());
        }
    }
}
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
        }
    }
}
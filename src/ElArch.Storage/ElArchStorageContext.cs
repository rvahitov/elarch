using ElArch.Storage.ReadModels;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ElArch.Storage
{
    public class ElArchStorageContext : DbContext
    {
        public ElArchStorageContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<DocumentTypeReadModel> DocumentTypes { get; set; }
    }
}
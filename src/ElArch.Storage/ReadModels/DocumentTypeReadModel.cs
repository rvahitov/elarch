using System;
using System.Collections.Generic;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElArch.Storage.ReadModels
{
    public class DocumentTypeReadModel
    {
        public DocumentTypeReadModel([NotNull] DocumentTypeId id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        [NotNull] public DocumentTypeId Id { get; }
        public DocumentTypeName Name { get; set; }

        public DateTimeOffset CreationTime { get; set; }
        public DateTimeOffset ModificationTime { get; set; }

        public int Version { get; set; }
        public virtual IList<FieldReadModel> Fields { get; set; }
    }

    internal sealed class DocumentTypeReadModelConfiguration : IEntityTypeConfiguration<DocumentTypeReadModel>
    {
        public void Configure(EntityTypeBuilder<DocumentTypeReadModel> builder)
        {
            builder.ToTable("DocumentType");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasConversion(id => id.Value, value => DocumentTypeId.With(value));
            builder.Property(e => e.Name)
                .HasConversion(name => name.Value, value => new DocumentTypeName(value));

            builder.Property(e => e.Version).ValueGeneratedNever().IsConcurrencyToken();
            builder.HasMany(e => e.Fields).WithOne()
                .HasForeignKey(f => f.DocumentTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
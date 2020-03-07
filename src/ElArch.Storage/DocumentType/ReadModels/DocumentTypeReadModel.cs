using System;
using System.Collections.Generic;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElArch.Storage.DocumentType.ReadModels
{
    public class DocumentTypeReadModel
    {
        public DocumentTypeId Id { get; internal set; }
        public int SequenceNumber { get; internal set; }
        public DocumentTypeName Name { get; internal set; }
        public DateTimeOffset CreationTime { get; internal set; }
        public DateTimeOffset ModificationTime { get; internal set; }
        public int Version { get; internal set; }

        public virtual ICollection<FieldReadModel> Fields { get; private set; } =
            new List<FieldReadModel>();
        
        public virtual ICollection<DocumentViewReadModel> DocumentViews { get; private set; } =
            new List<DocumentViewReadModel>();
    }

    internal sealed class DocumentTypeReadModelConfiguration : IEntityTypeConfiguration<DocumentTypeReadModel>
    {
        public void Configure(EntityTypeBuilder<DocumentTypeReadModel> builder)
        {
            builder.ToTable("DocumentType");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasConversion(id => id.Value, value => DocumentTypeId.With(value));
            builder.Property(e => e.SequenceNumber).ValueGeneratedOnAdd();
            builder.Property(e => e.Name)
                .HasConversion(name => name.Value, value => new DocumentTypeName(value));
            builder.Property(e => e.Version).IsConcurrencyToken();
            builder.HasMany(e => e.Fields)
                .WithOne().IsRequired().HasForeignKey(f => f.DocumentTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.DocumentViews).WithOne().IsRequired()
                .HasForeignKey(v => v.DocumentTypeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
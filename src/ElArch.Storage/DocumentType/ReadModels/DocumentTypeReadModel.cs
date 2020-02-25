using System;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElArch.Storage.DocumentType.ReadModels
{
    public class DocumentTypeReadModel
    {
        public int Id { get; internal set; }
        public DocumentTypeId AggregateId { get; internal set; }
        public DocumentTypeName Name { get; internal set; }
        public DateTimeOffset CreationTime { get; internal set; }
        public DateTimeOffset ModificationTime { get; internal set; }
        public int Version { get; internal set; }
    }

    internal sealed class DocumentTypeReadModelConfiguration : IEntityTypeConfiguration<DocumentTypeReadModel>
    {
        public void Configure(EntityTypeBuilder<DocumentTypeReadModel> builder)
        {
            builder.ToTable("DocumentType");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.AggregateId)
                .HasConversion(id => id.Value, value => DocumentTypeId.With(value));
            builder.Property(e => e.Name)
                .HasConversion(name => name.Value, value => new DocumentTypeName(value));
            builder.Property(e => e.Version).IsConcurrencyToken();
        }
    }
}
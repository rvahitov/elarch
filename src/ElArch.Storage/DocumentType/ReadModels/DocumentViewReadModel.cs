using System;
using System.Collections.Generic;
using System.Linq;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElArch.Storage.DocumentType.ReadModels
{
    public abstract class DocumentViewReadModel
    {
        public int Id { get; set; }
        public DocumentTypeId DocumentTypeId { get; set; }
        public virtual ICollection<ViewFieldReadModel> ViewFields { get; set; }

        public static DocumentViewReadModel FromDomainModel(IDocumentView documentView) =>
            documentView switch
            {
                null => throw new ArgumentNullException(nameof(documentView)),
                SearchView view => new SearchViewReadModel
                {
                    ViewFields = view.ViewFields.Select(ViewFieldReadModel.FromDomainModel).ToArray()
                },
                CardView view => new CardViewReadModel
                {
                    ViewFields = view.ViewFields.Select(ViewFieldReadModel.FromDomainModel).ToArray()
                },
                GridView view => new GridViewReadModel
                {
                    ViewFields = view.ViewFields.Select(ViewFieldReadModel.FromDomainModel).ToArray()
                },
                EditForm view => new EditFormReadModel
                {
                    ViewFields = view.ViewFields.Select(ViewFieldReadModel.FromDomainModel).ToArray()
                },
                _ => throw new NotSupportedException($"Document view of type {documentView.GetType()} is not supported")
            };
    }

    public class SearchViewReadModel : DocumentViewReadModel
    {
    }

    public class GridViewReadModel : DocumentViewReadModel
    {
    }

    public class CardViewReadModel : DocumentViewReadModel
    {
    }

    public class EditFormReadModel : DocumentViewReadModel
    {
    }

    internal sealed class DocumentViewReadModelConfiguration : IEntityTypeConfiguration<DocumentViewReadModel>
    {
        public void Configure(EntityTypeBuilder<DocumentViewReadModel> builder)
        {
            builder.ToTable("DocumentView");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.DocumentTypeId).HasConversion(e => e.Value, value => DocumentTypeId.With(value));
            builder.HasMany(e => e.ViewFields).WithOne().IsRequired().HasForeignKey(e => e.DocumentViewId).OnDelete(DeleteBehavior.Cascade);
            builder.HasDiscriminator<string>("ViewType")
                .HasValue<SearchViewReadModel>("Search")
                .HasValue<GridViewReadModel>("Grid")
                .HasValue<CardViewReadModel>("Card")
                .HasValue<EditFormReadModel>("Edit");
        }
    }
}
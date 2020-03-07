using System;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElArch.Storage.DocumentType.ReadModels
{
    public abstract class ViewFieldReadModel
    {
        public int DocumentViewId { get; set; }
        public FieldId FieldId { get; set; }
        public ViewFieldTitle Title { get; set; }
        public ViewOrder ViewOrder { get; set; }

        public static ViewFieldReadModel FromDomainModel(ViewField viewField) =>
            viewField switch
            {
                null => throw new ArgumentNullException(nameof(viewField)),
                SearchViewField field => new SearchViewFieldReadModel {FieldId = field.FieldId, Title = field.FieldTitle, ViewOrder = field.ViewOrder},
                GridViewField field => new GridViewFieldReadModel {FieldId = field.FieldId, Title = field.FieldTitle, ViewOrder = field.ViewOrder},
                CardViewField field => new CardViewFieldReadModel {FieldId = field.FieldId, Title = field.FieldTitle, ViewOrder = field.ViewOrder},
                EditFormField field => new EditFormFieldReadModel {FieldId = field.FieldId, Title = field.FieldTitle, ViewOrder = field.ViewOrder, IsReadOnly = field.IsReadOnly},
                _ => throw new NotSupportedException($"View field of type {viewField.GetType()} is not supported")
            };
    }

    public class SearchViewFieldReadModel : ViewFieldReadModel
    {
    }

    public class GridViewFieldReadModel : ViewFieldReadModel
    {
    }

    public class CardViewFieldReadModel : ViewFieldReadModel
    {
    }

    public class EditFormFieldReadModel : ViewFieldReadModel
    {
        public bool IsReadOnly { get; set; }
    }

    internal sealed class ViewFieldReadModelConfiguration : IEntityTypeConfiguration<ViewFieldReadModel>
    {
        public void Configure(EntityTypeBuilder<ViewFieldReadModel> builder)
        {
            builder.ToTable("DocumentViewField");
            builder.HasKey(e => new {e.DocumentViewId, e.FieldId});
            builder.Property(e => e.FieldId).HasConversion(id => id.Value, value => new FieldId(value));
            builder.Property(e => e.Title).HasConversion(t => t.Value, value => new ViewFieldTitle(value));
            builder.Property(e => e.ViewOrder).HasConversion(o => o.Value, value => new ViewOrder(value));
            builder.HasDiscriminator<string>("ViewType")
                .HasValue<SearchViewFieldReadModel>("Search")
                .HasValue<GridViewFieldReadModel>("Grid")
                .HasValue<CardViewFieldReadModel>("Card")
                .HasValue<EditFormFieldReadModel>("Edit");
        }
    }
}
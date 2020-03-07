using System;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public abstract class ViewField
    {
        public ViewField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder, [NotNull] ViewFieldTitle fieldTitle)
        {
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
            ViewOrder = viewOrder ?? throw new ArgumentNullException(nameof(viewOrder));
            FieldTitle = fieldTitle ?? throw new ArgumentNullException(nameof(fieldTitle));
        }

        public ViewField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder) : this(fieldId, viewOrder, new ViewFieldTitle(fieldId.Value))
        {
        }

        public FieldId FieldId { get; }
        public ViewOrder ViewOrder { get; }
        public ViewFieldTitle FieldTitle { get; }
    }

    public sealed class SearchViewField : ViewField
    {
        public SearchViewField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder, [NotNull] ViewFieldTitle fieldTitle) : base(fieldId, viewOrder, fieldTitle)
        {
        }

        public SearchViewField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder) : base(fieldId, viewOrder)
        {
        }
    }

    public sealed class GridViewField : ViewField
    {
        public GridViewField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder, [NotNull] ViewFieldTitle fieldTitle) : base(fieldId, viewOrder, fieldTitle)
        {
        }

        public GridViewField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder) : base(fieldId, viewOrder)
        {
        }
    }

    public sealed class CardViewField : ViewField
    {
        public CardViewField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder, [NotNull] ViewFieldTitle fieldTitle) : base(fieldId, viewOrder, fieldTitle)
        {
        }

        public CardViewField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder) : base(fieldId, viewOrder)
        {
        }
    }

    public sealed class EditFormField : ViewField
    {
        public EditFormField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder, [NotNull] ViewFieldTitle fieldTitle) : base(fieldId, viewOrder, fieldTitle)
        {
        }

        public EditFormField([NotNull] FieldId fieldId, [NotNull] ViewOrder viewOrder) : base(fieldId, viewOrder)
        {
        }

        public bool IsReadOnly { get; set; }
    }
}
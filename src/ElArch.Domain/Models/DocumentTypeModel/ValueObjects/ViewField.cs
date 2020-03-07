using System;
using System.Collections.Generic;
using System.Linq;
using Akkatecture.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public abstract class ViewField : ValueObject
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return GetType();
            yield return FieldId;
            yield return ViewOrder;
            yield return FieldTitle;
        }
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return base.GetEqualityComponents().Concat(new object[] {IsReadOnly});
        }
    }
}
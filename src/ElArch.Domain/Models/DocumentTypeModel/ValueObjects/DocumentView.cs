using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    public interface IDocumentView
    {
        IEnumerable<ViewField> ViewFields { get; }
    }

    public abstract class DocumentView<TDocumentView, TViewField> : IDocumentView, IEquatable<DocumentView<TDocumentView, TViewField>>
        where TDocumentView : DocumentView<TDocumentView, TViewField>
        where TViewField : ViewField
    {
        protected DocumentView([NotNull] ImmutableList<TViewField> viewFields)
        {
            ViewFields = viewFields ?? throw new ArgumentNullException(nameof(viewFields));
        }

        public ImmutableList<TViewField> ViewFields { get; }

        IEnumerable<ViewField> IDocumentView.ViewFields => ViewFields;

        public bool Equals(DocumentView<TDocumentView, TViewField>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (ViewFields.Count != other.ViewFields.Count) return false;
            return ViewFields.OrderBy(f => f.FieldId).Zip(other.ViewFields.OrderBy(f => f.FieldId), (f1, f2) => f1.Equals(f2)).All(b => b);
        }

        [NotNull]
        public TDocumentView AddViewField([NotNull] TViewField viewField)
        {
            if (viewField == null) throw new ArgumentNullException(nameof(viewField));
            if (ViewFields.Any(f => f.FieldId == viewField.FieldId))
                throw new ApplicationException($"Document view already has view field {viewField.FieldId}");
            var viewFields = ViewFields.Add(viewField);
            return (TDocumentView) Activator.CreateInstance(typeof(TDocumentView), viewFields);
        }

        [NotNull]
        public TDocumentView RemoveViewField([NotNull] TViewField viewField)
        {
            if (viewField == null) throw new ArgumentNullException(nameof(viewField));
            if (ViewFields.Count(f => f.FieldId == viewField.FieldId) == 0) return (TDocumentView) this;
            var viewFields = ViewFields.RemoveAll(f => f.FieldId == viewField.FieldId);
            return (TDocumentView) Activator.CreateInstance(typeof(TDocumentView), viewFields);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DocumentView<TDocumentView, TViewField>) obj);
        }

        public override int GetHashCode()
        {
            var hasCode = new HashCode();
            foreach (var f in ViewFields.OrderBy(f => f.FieldId)) hasCode.Add(f);

            return hasCode.ToHashCode();
        }
    }

    public sealed class SearchView : DocumentView<SearchView, SearchViewField>
    {
        public SearchView([NotNull] ImmutableList<SearchViewField> viewFields) : base(viewFields)
        {
        }

        public SearchView() : base(ImmutableList<SearchViewField>.Empty)
        {
        }
    }

    public sealed class GridView : DocumentView<GridView, GridViewField>
    {
        public GridView([NotNull] ImmutableList<GridViewField> viewFields) : base(viewFields)
        {
        }

        public GridView() : base(ImmutableList<GridViewField>.Empty)
        {
        }
    }

    public sealed class CardView : DocumentView<CardView, CardViewField>
    {
        public CardView([NotNull] ImmutableList<CardViewField> viewFields) : base(viewFields)
        {
        }

        public CardView() : base(ImmutableList<CardViewField>.Empty)
        {
        }
    }

    public sealed class EditForm : DocumentView<EditForm, EditFormField>
    {
        public EditForm([NotNull] ImmutableList<EditFormField> viewFields) : base(viewFields)
        {
        }

        public EditForm() : base(ImmutableList<EditFormField>.Empty)
        {
        }
    }
}
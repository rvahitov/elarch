using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akkatecture.Core;
using Akkatecture.Entities;
using Akkatecture.ValueObjects;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;
using MassTransit;
using Newtonsoft.Json;

namespace ElArch.Domain.Models.DocumentTypeModel.Entities
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class DocumentItemTypeId : Identity<DocumentItemTypeId>
    {
        public DocumentItemTypeId(string value) : base(value)
        {
        }

        public static DocumentItemTypeId CreateNew() => With(NewId.NextGuid());
    }

    public sealed class DocumentItemType : Entity<DocumentItemTypeId>
    {
        [JsonConstructor]
        private DocumentItemType(DocumentItemTypeId id, DocumentItemTypeName name, IReadOnlyCollection<IField> fields) : this(id, name, fields.ToImmutableList())
        {
        }

        public DocumentItemType(DocumentItemTypeId id, DocumentItemTypeName name, ImmutableList<IField> fields) : base(id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public DocumentItemType(DocumentItemTypeId id, DocumentItemTypeName name) : this(id, name, ImmutableList<IField>.Empty)
        {
        }

        public DocumentItemTypeName Name { get; }
        public ImmutableList<IField> Fields { get; }

        public DocumentItemType AddField([NotNull] IField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (Fields.Any(f => f.FieldId == field.FieldId))
                throw new ApplicationException($"DocumentItemType already has field {field.FieldId}");
            return new DocumentItemType(Id, Name, Fields.Add(field));
        }

        public DocumentItemType RemoveField(IField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (Fields.Count == 0 || Fields.All(f => f.FieldId != field.FieldId)) return this;
            return new DocumentItemType(Id, Name, Fields.RemoveAll(f => f.FieldId == field.FieldId));
        }
    }
}
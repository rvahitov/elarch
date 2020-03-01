using System;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using ElArch.WebApi.Infrastructure;
using FluentValidation;
using Newtonsoft.Json.Linq;

namespace ElArch.WebApi.Controllers.DocumentTypes.Dto
{
    public enum FieldType
    {
        Boolean,
        Integer,
        Decimal,
        DateTime,
        Text
    }

    public sealed class FieldDto
    {
        public string FieldId { get; set; }
        public FieldType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public JValue MinValue { get; set; }
        public JValue MaxValue { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }

        public IField ToDomainModel()
        {
            var fieldId = new FieldId(FieldId);
            return FieldType switch
            {
                FieldType.Boolean => new BooleanField(fieldId).IsRequired(IsRequired),
                FieldType.Integer => new IntegerField(fieldId).IsRequired(IsRequired).MinValue(MinValue?.Value<int?>()).MaxValue(MaxValue?.Value<int?>()),
                FieldType.Decimal => new DecimalField(fieldId).IsRequired(IsRequired).MinValue(MinValue?.Value<decimal?>()).MaxValue(MaxValue?.Value<decimal>()),
                FieldType.DateTime => new DateTimeField(fieldId).IsRequired(IsRequired).MinValue(MinValue?.Value<DateTime?>()).MaxValue(MaxValue?.Value<DateTime?>()),
                FieldType.Text => new TextField(fieldId).IsRequired(IsRequired).MinLength(MinLength).MaxLength(MaxLength),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public sealed class FieldDtoValidator : AbstractValidator<FieldDto>
    {
        public FieldDtoValidator()
        {
            RuleFor(f => f.FieldId).NotEmpty().MaximumLength(128);
            When(f => f.FieldType == FieldType.Integer, () =>
            {
                RuleFor(f => f.MinValue).JValueShouldBeOfType(JTokenType.Integer)
                    .WithMessage("MinValue should be null or integer");
                RuleFor(f => f.MaxValue).JValueShouldBeOfType(JTokenType.Integer)
                    .WithMessage("MaxValue should be null or integer");
            });
            When(f => f.FieldType == FieldType.Decimal, () =>
            {
                RuleFor(f => f.MinValue).JValueShouldBeOfType(JTokenType.Float)
                    .WithMessage("MinValue should be null or float");
                RuleFor(f => f.MaxValue).JValueShouldBeOfType(JTokenType.Float)
                    .WithMessage("MaxValue should be null or integer");
            });
            When(f => f.FieldType == FieldType.DateTime, () =>
            {
                RuleFor(f => f.MinValue).Must(v => v?.Value == null || v.Value<DateTime?>() != null)
                    .WithMessage("MinValue should be null or DateTimeOffset");
                RuleFor(f => f.MaxValue).Must(v => v?.Value == null || v.Value<DateTime?>() != null)
                    .WithMessage("MaxValue should be null or DateTimeOffset");
            });
        }
    }
}
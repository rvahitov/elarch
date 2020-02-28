using System;
using FluentValidation;

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
        [System.Text.Json.Serialization.JsonConverter(typeof())]
        public object MinValue { get; set; }
        public object MaxValue { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }

    public sealed class FieldDtoValidator : AbstractValidator<FieldDto>
    {
        public FieldDtoValidator()
        {
            RuleFor(f => f.FieldId).NotEmpty().MaximumLength(128);
            RuleFor(f => f).Custom((field, ctx) =>
            {
                if (field.MinValue is null && field.MaxValue is null) return;
                switch (field.FieldType, field.MinValue, field.MaxValue)
                {
                    case (FieldType.Integer, int _, null):
                    case (FieldType.Integer, null, int _):
                    case (FieldType.Integer, int min, int max) when min < max:
                        return;
                    case (FieldType.Integer, _, _):
                        ctx.AddFailure("Invalid Min/Max value for integer field or min value is not less than max value");
                        return;
                    case (FieldType.Decimal, decimal _, null):
                    case (FieldType.Decimal, null, decimal _):
                    case (FieldType.Decimal, decimal min, decimal max) when min < max:
                        return;
                    case (FieldType.Decimal, _, _):
                        ctx.AddFailure("Invalid Min/Max value for decimal field or min value is not less than max value");
                        return;
                    case (FieldType.DateTime, DateTimeOffset _, null):
                    case (FieldType.DateTime, null, DateTimeOffset _):
                    case (FieldType.DateTime, DateTimeOffset min, DateTimeOffset max) when min < max:
                        return;
                    case (FieldType.DateTime, _, _):
                        ctx.AddFailure("Invalid Min/Max value for date-time field or min value is not less than max value");
                        return;
                }
            });
        }
    }
}
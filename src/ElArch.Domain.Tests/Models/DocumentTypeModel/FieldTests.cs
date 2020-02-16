using System;
using AutoFixture.Xunit2;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using Xunit;

namespace ElArch.Domain.Tests.Models.DocumentTypeModel
{
    public sealed class FieldTests
    {
        private readonly FieldId _fieldId = new FieldId("test");

        [Fact]
        public void ConstructorShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new BooleanField(null, null));
            Assert.Throws<ArgumentNullException>(() => new BooleanField(_fieldId, null));
        }

        [Fact]
        public void FieldShouldContainValueOfTypeValidator()
        {
            var decimalField = new DecimalField(_fieldId);
            Assert.NotEmpty(decimalField.Validators);
            Assert.Contains(decimalField.Validators, v => v is ValueOfTypeValidator<decimal>);
            var dateTimeField = new DateTimeField(_fieldId);
            Assert.NotEmpty(dateTimeField.Validators);
            Assert.Contains(dateTimeField.Validators, v => v is ValueOfTypeValidator<DateTimeOffset>);
            var textField = new TextField(_fieldId);
            Assert.NotEmpty(textField.Validators);
            Assert.Contains(textField.Validators, v => v is ValueOfTypeValidator<string>);
            var stringField = new StringField(_fieldId);
            Assert.NotEmpty(stringField.Validators);
            Assert.Contains(stringField.Validators, v => v is ValueOfTypeValidator<string>);
        }

        [Fact]
        public void BooleanFieldTests()
        {
            var field = new BooleanField(_fieldId);
            Assert.NotEmpty(field.Validators);
            Assert.Contains(field.Validators, v => v is ValueOfTypeValidator<bool>);
            Assert.DoesNotContain(field.Validators, v => v is FieldRequiredValueValidator);
            Assert.False(field.IsRequired());
            field = field.IsRequired(true);
            Assert.NotEmpty(field.Validators);
            Assert.Contains(field.Validators, v => v is ValueOfTypeValidator<bool>);
            Assert.Contains(field.Validators, v => v is FieldRequiredValueValidator);
            Assert.True(field.IsRequired());
        }

        [AutoData]
        [Theory]
        public void IntegerFieldTests(int minValue)
        {
            var field = new IntegerField(_fieldId);
            Assert.NotEmpty(field.Validators);
            Assert.Contains(field.Validators, v => v is ValueOfTypeValidator<int>);
            Assert.DoesNotContain(field.Validators, v => v is FieldRequiredValueValidator);
            Assert.DoesNotContain(field.Validators, v => v is FieldMinValueValidator<int>);
            Assert.DoesNotContain(field.Validators, v => v is FieldMaxValueValidator<int>);
            Assert.False(field.IsRequired());
            Assert.Null(field.MinValue());
            Assert.Null(field.MaxValue());
            field = field.IsRequired(true);
            Assert.Contains(field.Validators, v => v is FieldRequiredValueValidator);
            Assert.True(field.IsRequired());
            field = field.MinValue(minValue);
            Assert.Contains(field.Validators, v => v is FieldMinValueValidator<int>);
            Assert.Equal(minValue, field.MinValue());
            var maxValue = minValue + 200;
            field = field.MaxValue(maxValue);
            Assert.Contains(field.Validators, v => v is FieldMaxValueValidator<int>);
            Assert.Equal(maxValue, field.MaxValue());
            field = field.MaxValue(null);
            Assert.DoesNotContain(field.Validators, v => v is FieldMaxValueValidator<int>);
            Assert.Null(field.MaxValue());
            field = field.MinValue(null);
            Assert.DoesNotContain(field.Validators, v => v is FieldMinValueValidator<int>);
            Assert.Null(field.MinValue());
        }

        [AutoData]
        [Theory]
        public void StringFieldTests(int minLength)
        {
            var field = new StringField(_fieldId);
            Assert.Null(field.MinLength());
            Assert.Null(field.MaxLength());
            field = field.MinLength(minLength);
            Assert.Equal(minLength, field.MinLength());
            var maxLength = minLength + 100;
            field = field.MaxLength(maxLength);
            Assert.Equal(maxLength, field.MaxLength());
            field = field.MaxLength(null);
            Assert.Null(field.MaxLength());
            field = field.MinLength(null);
            Assert.Null(field.MinLength());
        }
    }
}
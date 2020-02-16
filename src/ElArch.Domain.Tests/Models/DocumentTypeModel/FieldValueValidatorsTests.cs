using AutoFixture.Xunit2;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using Xunit;

namespace ElArch.Domain.Tests.Models.DocumentTypeModel
{
    public sealed class FieldValueValidatorsTests
    {
        [Fact]
        public void RequiredValueValidatorShouldReturnError()
        {
            var validator = new FieldRequiredValueValidator();
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, null);
            Assert.False(result.IsSuccess());
            Assert.False(string.IsNullOrEmpty(result.AsT1.Value));
        }

        [AutoData]
        [Theory]
        public void RequiredValueValidatorShouldReturnSuccess(string checkValue)
        {
            var validator = new FieldRequiredValueValidator();
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, checkValue);
            Assert.True(result.IsSuccess());
            Assert.Equal(checkValue, result.AsT0);
        }

        [AutoData]
        [Theory]
        public void ValueOfTypeValidatorShouldReturnError(int checkValue)
        {
            var validator = new ValueOfTypeValidator<string>();
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, checkValue);
            Assert.False(result.IsSuccess());
            Assert.False(string.IsNullOrEmpty(result.AsT1.Value));
        }

        [AutoData]
        [Theory]
        public void ValueOfTypeValidatorShouldReturnSuccess(int checkValue)
        {
            var validator = new ValueOfTypeValidator<int>();
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, null);
            Assert.True(result.IsSuccess());
            Assert.Null(result.AsT0);

            result = validator.Validate(fieldId, checkValue);
            Assert.True(result.IsSuccess());
            Assert.Equal(checkValue, result.AsT0);
        }

        [AutoData]
        [Theory]
        public void FieldMinValueValidatorShouldReturnError(int minValue)
        {
            var validator = new FieldMinValueValidator<int>(minValue);
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, minValue - 10);
            Assert.False(result.IsSuccess());
            Assert.False(string.IsNullOrEmpty(result.AsT1.Value));
        }

        [AutoData]
        [Theory]
        public void FieldMinValueValidatorShouldReturnSuccess(int minValue)
        {
            var validator = new FieldMinValueValidator<int>(minValue);
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, null);
            Assert.True(result.IsSuccess());
            Assert.Null(result.AsT0);
            var checkValue = minValue + 15;
            result = validator.Validate(fieldId, checkValue);
            Assert.True(result.IsSuccess());
            Assert.Equal(checkValue, result.AsT0);
        }

        [AutoData]
        [Theory]
        public void FieldMaxValueValidatorShouldReturnError(int maxValue)
        {
            var validator = new FieldMaxValueValidator<int>(maxValue);
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, maxValue + 30);
            Assert.False(result.IsSuccess());
            Assert.False(string.IsNullOrEmpty(result.AsT1.Value));
        }

        [AutoData]
        [Theory]
        public void FieldMaxValueValidatorShouldReturnSuccess(int maxValue)
        {
            var validator = new FieldMaxValueValidator<int>(maxValue);
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, null);
            Assert.True(result.IsSuccess());
            Assert.Null(result.AsT0);
            var checkValue = maxValue - 150;
            result = validator.Validate(fieldId, checkValue);
            Assert.True(result.IsSuccess());
            Assert.Equal(checkValue, result.AsT0);
        }

        [Fact]
        public void FieldValueMinLengthValidatorShouldReturnError()
        {
            var validator = new FieldValueMinLengthValidator(130);
            var fieldId = new FieldId("Unknown");
            var checkValue = new string('a', 100);
            var result = validator.Validate(fieldId, checkValue);
            Assert.False(result.IsSuccess());
            Assert.False(string.IsNullOrEmpty(result.AsT1.Value));
        }

        [Fact]
        public void FieldValueMinLengthValidatorShouldReturnSuccess()
        {
            var validator = new FieldValueMinLengthValidator(130);
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, null);
            Assert.True(result.IsSuccess());
            Assert.Null(result.AsT0);
            var checkValue = new string('a', 140);
            result = validator.Validate(fieldId, checkValue);
            Assert.True(result.IsSuccess());
            Assert.Equal(checkValue, result.AsT0);
        }
        
        [Fact]
        public void FieldValueMaxLengthValidatorShouldReturnError()
        {
            var validator = new FieldValueMaxLengthValidator(130);
            var fieldId = new FieldId("Unknown");
            var checkValue = new string('a', 140);
            var result = validator.Validate(fieldId, checkValue);
            Assert.False(result.IsSuccess());
            Assert.False(string.IsNullOrEmpty(result.AsT1.Value));
        }

        [Fact]
        public void FieldValueMaxLengthValidatorShouldReturnSuccess()
        {
            var validator = new FieldValueMaxLengthValidator(130);
            var fieldId = new FieldId("Unknown");
            var result = validator.Validate(fieldId, null);
            Assert.True(result.IsSuccess());
            Assert.Null(result.AsT0);
            var checkValue = new string('a', 130);
            result = validator.Validate(fieldId, checkValue);
            Assert.True(result.IsSuccess());
            Assert.Equal(checkValue, result.AsT0);
        }
    }
}
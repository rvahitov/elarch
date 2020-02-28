using FluentValidation;
using Newtonsoft.Json.Linq;

namespace ElArch.WebApi.Infrastructure
{
    public static class Validators
    {
        public static IRuleBuilderOptions<T, JValue> JValueShouldBeOfType<T>(this IRuleBuilder<T, JValue> ruleBuilder, JTokenType type, bool canBeNull = true)
        {
            return ruleBuilder.Must(v => (v.Value == null && canBeNull) || v.Type == type);
        }
    }
}
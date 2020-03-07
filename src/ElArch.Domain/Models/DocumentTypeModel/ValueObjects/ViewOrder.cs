using Akkatecture.ValueObjects;
using Newtonsoft.Json;

namespace ElArch.Domain.Models.DocumentTypeModel.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class ViewOrder : SingleValueObject<int>
    {
        public ViewOrder(int value) : base(value)
        {
        }

        public static ViewOrder Empty { get; } = new ViewOrder(0);
    }
}
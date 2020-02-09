using System.Collections.Generic;
using System.Linq;
using Akkatecture.Specifications;
using OneOf;
using OneOf.Types;

namespace ElArch.Domain.Core.Extensions
{
    public static class SpecificationExtensions
    {
        public static OneOf<T, Error<IEnumerable<string>>> Check<T>(this ISpecification<T> specification, T target)
        {
            var errors = specification.WhyIsNotSatisfiedBy(target).ToArray();
            return errors.Length == 0
                ? OneOf<T, Error<IEnumerable<string>>>.FromT0(target)
                : OneOf<T, Error<IEnumerable<string>>>.FromT1(new Error<IEnumerable<string>>(errors));
        }
    }
}
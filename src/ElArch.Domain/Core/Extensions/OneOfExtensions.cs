#nullable enable
using System;
using System.Collections.Generic;
using Akkatecture.Aggregates.ExecutionResults;
using OneOf;
using OneOf.Types;

namespace ElArch.Domain.Core.Extensions
{
    public static class OneOfExtensions
    {
        public static bool IsSuccess<T, TError>(this OneOf<T, Error<TError>> oneOf) => oneOf.IsT0;

        public static OneOf<TOut, Error<TError>> Map<TIn, TOut, TError>(this OneOf<TIn, Error<TError>> oneOf, Func<TIn, TOut> mapper)
        {
            return oneOf.IsSuccess()
                ? OneOf<TOut, Error<TError>>.FromT0(mapper(oneOf.AsT0))
                : OneOf<TOut, Error<TError>>.FromT1(oneOf.AsT1);
        }

        public static OneOf<TLeft, TRight> ApplyOnLeft<TLeft, TRight>(this OneOf<TLeft, TRight> oneOf, Action<TLeft> action)
        {
            if (oneOf.IsT0) action(oneOf.AsT0);
            return oneOf;
        }

        public static IExecutionResult ToExecutionResult<T>(this OneOf<T, Error<string>> oneOf) =>
            oneOf.IsSuccess()
                ? ExecutionResult.Success()
                : ExecutionResult.Failed(oneOf.AsT1.Value);

        public static IExecutionResult ToExecutionResult<T>(this OneOf<T, Error<IEnumerable<string>>> oneOf) =>
            oneOf.IsSuccess()
                ? ExecutionResult.Success()
                : ExecutionResult.Failed(oneOf.AsT1.Value);
    }
}
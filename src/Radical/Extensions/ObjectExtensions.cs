using Radical.Conversions;
using System;

namespace Radical
{
    /// <summary>
    /// Adds behaviors to the base object class.
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        /// Exposes the specified object, trying to cast it to the destination type,
        /// in order to provide a fluent interface based access to objects that does 
        /// not support fluent interfaces.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="obj">The current object.</param>
        /// <param name="interceptor">The delegate used to intercept the exposition.</param>
        /// <returns>The originally exposed object.</returns>
        public static TSource InterceptAs<TSource, TDestination>(this TSource obj, Action<TDestination> interceptor)
            where TSource : class
            where TDestination : class
        {
            obj.As<TDestination>
            (
                d =>
                {
                    interceptor(d);
                },

                () =>
                {
                    throw new InvalidCastException();
                }
            );

            return obj;
        }


        /// <summary>
        /// Executes the supplied delegate only if the given predicate returns true.
        /// </summary>
        /// <typeparam name="T">The type of the intercepted object.</typeparam>
        /// <param name="obj">The current intercepted object.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="thenAction">The action if the condition evaluates to true.</param>
        /// <returns>The intercepted object.</returns>
        public static T If<T>(this T obj, Predicate<T> condition, Action<T> thenAction) where T : class
        {
            return If(obj, condition, thenAction, o => { });
        }

        /// <summary>
        /// Executes the supplied delegates based on the evaluation of the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of the intercepted object.</typeparam>
        /// <param name="obj">The current intercepted object.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="thenAction">The action if the condition evaluates to true.</param>
        /// <param name="elseAction">The action if the condition evaluates to false.</param>
        /// <returns>The intercepted object.</returns>
        public static T If<T>(this T obj, Predicate<T> condition, Action<T> thenAction, Action<T> elseAction) where T : class
        {
            if (condition(obj))
            {
                thenAction(obj);
            }
            else
            {
                elseAction(obj);
            }

            return obj;
        }



#pragma warning disable 1591


        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
          where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }










        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator)
        {
            return input.Return(evaluator, () => default(TResult), i => Object.ReferenceEquals(i, null));
        }

        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, TResult defaultValueOnNullInput)
        {
            return input.Return(evaluator, () => defaultValueOnNullInput, i => Object.ReferenceEquals(i, null));
        }

        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, Func<TResult> defaultValueOnNullInput)
        {
            return input.Return(evaluator, defaultValueOnNullInput, i => Object.ReferenceEquals(i, null));
        }

        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, Predicate<TInput> failureEvaluator, Func<TResult> failureValue)
        {
            return input.Return(evaluator, failureValue, failureEvaluator);
        }

        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator)
        {
            return input.Return(evaluator, () => default(TResult), obj => Object.ReferenceEquals(obj, null));
        }

        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, TResult defaultValueOnNullInput)
        {
            return input.Return(evaluator, () => defaultValueOnNullInput, obj => Object.ReferenceEquals(obj, null));
        }

        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, TResult failureValue, Predicate<TInput> failureEvaluator)
        {
            return input.Return(evaluator, () => failureValue, failureEvaluator);
        }

#pragma warning restore 1591

        /// <summary>
        /// Compares the input value against null (ReferenceEquals), if the supplied value is not null returns
        /// the value using the specified evaluator otherwise the one provided by the supplied default value func.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="input">The input value.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <param name="defaultValueOnNullInput">The default value on null input.</param>
        /// <returns>
        /// The evaluated input value; otherwise, if the input value reference equals null, returns the default value.
        /// </returns>
        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, Func<TResult> defaultValueOnNullInput)
        {
            return input.Return(evaluator, defaultValueOnNullInput, obj => Object.ReferenceEquals(obj, null));
        }

        /// <summary>
        /// Evaluates the given input value using the supplied failure evaluator 
        /// and returns the value using the specified evaluator or the one provided by the
        /// supplied failure value.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="input">The input value.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <param name="failureValue">The failure value.</param>
        /// <param name="failureEvaluator">The failure evaluator.</param>
        /// <returns>The evaluated input value; otherwise, if the failure evaluator fails, return the failure value.</returns>
        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator, Func<TResult> failureValue, Predicate<TInput> failureEvaluator)
        {
            if (failureEvaluator(input))
            {
                return failureValue();
            }

            return evaluator(input);
        }

        /// <summary>
        /// Executes the specified action if the input value is not null (ReferenceEquals).
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="input">The input value.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>The input value.</returns>
        public static TInput Do<TInput>(this TInput input, Action<TInput> action)
        {
            return input.Do(i => Object.ReferenceEquals(i, null), action);
        }

        /// <summary>
        /// Executes the specified action if the supplied failure evaluator
        /// evaluates to false.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="input">The input value.</param>
        /// <param name="failureEvaluator">The failure evaluator.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>
        /// The input value.
        /// </returns>
        public static TInput Do<TInput>(this TInput input, Predicate<TInput> failureEvaluator, Action<TInput> action)
        {
            if (!failureEvaluator(input))
            {
                action(input);
            }

            return input;
        }
    }
}

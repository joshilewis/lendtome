using System;
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;

namespace Joshilewis.Testing
{
    public static class TestValueEqualityHelpers
    {
        private static Dictionary<Type, Action<object, object>> valueEqualityActions =
            new Dictionary<Type, Action<object, object>>();

        public static void SetValueEqualityActions(params EqualityAction[] equalityActions )
        {
            foreach (var equalityAction in equalityActions)
            {
                valueEqualityActions.Add(equalityAction.Type, (actual, expected) => equalityAction.Invoke(actual, expected));
            }
        }

        public static void CompareValueEquality<T>(T actual, T expected)
        {
            if (!valueEqualityActions.ContainsKey(actual.GetType()))
            {
                if (!actual.Equals(expected)) throw new AssertionException("Values are not equal");
            }
            valueEqualityActions[actual.GetType()](actual, expected);
        }

        public static bool ShouldEqual(this HttpResponseMessage actual, HttpResponseMessage expected)
        {
            Assert.That(actual.StatusCode, Is.EqualTo(expected.StatusCode));
            Assert.That(actual.ReasonPhrase, Is.EqualTo(expected.ReasonPhrase));

            return true;
        }


    }

    public abstract class EqualityAction
    {
        public abstract void Invoke(object actual, object expected);
        public abstract Type Type { get; }
    }

    public class EqualityAction<T> : EqualityAction
    {
        private readonly Action<T, T> action;
        public override void Invoke(object actual, object expected)
        {
            Invoke((T) actual, (T) expected);
        }

        public override Type Type => typeof(T);

        public EqualityAction(Action<T, T> action)
        {
            this.action = action;
        }

        public void Invoke(T t1, T t2)
        {
            action(t1, t2);
        }
    }
}
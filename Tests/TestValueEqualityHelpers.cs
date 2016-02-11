using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public static class TestValueEqualityHelpers
    {
        private static Dictionary<Type, Action<object, object>> valueEqualityActions =
            new Dictionary<Type, Action<object, object>>();

        public static void SetValueEqualityActions(Dictionary<Type, Action<object, object>> valueEqualityMap)
        {
            valueEqualityActions = valueEqualityMap;
        }

        public static void CompareValueEquality<T>(T actual, T expected)
        {
            if (!valueEqualityActions.ContainsKey(actual.GetType()))
            {
                if (!actual.Equals(expected)) throw new AssertionException("Values are not equal");
            }
            valueEqualityActions[actual.GetType()](actual, expected);
        }
    }
}
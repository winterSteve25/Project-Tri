using System;

namespace Utils
{
    public enum ComparisonOperator
    {
        GreaterThan,
        LessThan,
        EqualsTo
    }

    public static class ComparisonOperatorExtension
    {
        public static bool Compare<T>(this ComparisonOperator comparisonOperator, T a, T b) where T : IComparable
        {
            return comparisonOperator switch
            {
                ComparisonOperator.GreaterThan => a.CompareTo(b) > 0,
                ComparisonOperator.LessThan => a.CompareTo(b) < 0,
                ComparisonOperator.EqualsTo => a.CompareTo(b) == 0,
                _ => throw new ArgumentOutOfRangeException(nameof(comparisonOperator), comparisonOperator, null)
            };
        }
    }
}
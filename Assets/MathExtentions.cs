using System;
using UnityEngine;

public struct MathUtility
{
    /// <summary>
    /// Calculates the percentage of the value between the values min and max.
    /// The value is clamped between min and max.
    /// </summary>
    /// <param name="value">the input value to calculate</param>
    /// <param name="min">The minimum value (0%)</param>
    /// <param name="max">The maximum vlaue (100%)</param>
    /// <returns>A value between 0 and 1 representing 0 to 100%</returns>
    public static float PercentageBetween(float value, float min, float max, bool oneOnNaN = false)
    {
        float clamppedValue = Mathf.Clamp(value, min, max);
        float result = (clamppedValue - min) / (max - min);

        return float.IsNaN(result) ? (oneOnNaN ? 1 : 0) : result;
    }
}
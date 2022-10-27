using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public static class Extensions
{
    private static readonly Regex regex = new Regex("([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z0-9]+)", RegexOptions.Compiled);

    /// <summary>
    /// Splits a PascalCase string using Regex. Splits numbers as well.
    /// </summary>
    public static string SplitPascalCase(this string value)
    {
        return regex.Replace(value, " $1").Trim();
    }

    /// <summary>
    /// Returns a Vector2 constructed using this Vector3's x and y.
    /// </summary>
    public static Vector2 ToV2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    /// <summary>
    /// Returns the result of this Rect inflated by the provided amount.
    /// </summary>
    public static Rect Inflated(this Rect r, float x, float y)
    {
        return new Rect(r.x - x, r.y - y, r.width + x * 2f, r.height + y * 2f);
    }

    /// <summary>
    /// Returns the result of this RectInt inflated by the provided amount.
    /// </summary>
    public static RectInt Inflated(this RectInt r, int x, int y)
    {
        return new RectInt(r.x - x, r.y - y, r.width + x * 2, r.height + y * 2);
    }

    /// <summary>
    /// Returns a new float to be used, instead of this float, as argument t in any lerp function.
    /// Perfect accuracy. Subtle effect. See plotted function: https://www.desmos.com/calculator/ktcwf5obja
    /// </summary>
    public static float LerpValueSmoothstep(this float x)
    {
        // Classic smoothstep function

        x = Mathf.Clamp01(x);
        return x * x * (3.0f - 2.0f * x);
    }

    /// <summary>
    /// Returns a new float to be used, instead of this float, as argument t in any lerp function.
    /// Perfect accuracy. Customizable effect. See plotted function: https://www.desmos.com/calculator/ktcwf5obja
    /// </summary>
    /// <param name="x">Input value (0 to 1)</param>
    /// <param name="a">Function amplitude (-0.98 to 0.98)</param>
    /// <param name="p">Position of function separator (0 to 1)</param>
    public static float LerpValueCustomSmoothstep(this float x, float a = 0.5f, float p = 0.5f)
    {
        x = Mathf.Clamp01(x);
        a = Mathf.Clamp(a, -0.98f, 0.98f);
        p = Mathf.Clamp01(p);

        float c = 2 / (1 - a) - 1; // Function amplitude

        float F(float x, float n) // Function
        {
            return Mathf.Pow(x, c) / Mathf.Pow(n, c - 1);
        }

        return x < p ? F(x, p) : 1 - F(1 - x, 1 - p); // Output
    }

    /// <summary>
    /// Returns the next value of this enum.
    /// </summary>
    public static T GetNext<T>(this T value) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] array = (T[])Enum.GetValues(value.GetType());
        int i = Array.IndexOf(array, value) + 1;

        return (i == array.Length) ? array[0] : array[i];
    }

    /// <summary>
    /// Returns the previous value of this enum.
    /// </summary>
    public static T GetPrevious<T>(this T value) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] array = (T[])Enum.GetValues(value.GetType());
        int i = Array.IndexOf(array, value) - 1;

        return (i == -1) ? array[array.Length - 1] : array[i];
    }

    /// <summary>
    /// Sets the value of this enum to the next value and returns it.
    /// </summary>
    public static T SetNext<T>(this ref T value) where T : struct
    {
        return value = value.GetNext();
    }

    /// <summary>
    /// Sets the value of this enum to the previous value and returns it.
    /// </summary>
    public static T SetPrevious<T>(this ref T value) where T : struct
    {
        return value = value.GetPrevious();
    }

    /// <summary>
    /// Returns a Vector2 as the result of rotating this Vector2 around origo by the provided radians.
    /// </summary>
    public static Vector2 RotateAroundZero(this Vector2 point, float radians)
    {
        return new Vector2(
            Mathf.Cos(radians) * point.x - Mathf.Sin(radians) * point.y,
            Mathf.Sin(radians) * point.x + Mathf.Cos(radians) * point.y);
    }

    /// <summary>
    /// Returns a Vector2 as the result of rotating this Vector2 around the provided pivot point by the provided radians.
    /// </summary>
    public static Vector2 RotateAroundPivot(this Vector2 point, Vector2 pivot, float radians)
    {
        return new Vector2(
            Mathf.Cos(radians) * (point.x - pivot.x) - Mathf.Sin(radians) * (point.y - pivot.y) + pivot.x,
            Mathf.Sin(radians) * (point.x - pivot.x) + Mathf.Cos(radians) * (point.y - pivot.y) + pivot.y);
    }

    /// <summary>
    /// Shuffles this list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> values)
    {
        for (int i = 0; i < values.Count; ++i)
        {
            T temp = values[i];
            int randomIndex = Random.Range(i, values.Count);
            values[i] = values[randomIndex];
            values[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Returns whether or not the two rectangles overlap and their overlapping area.
    /// </summary>
    public static bool Overlaps(this RectInt rect, RectInt other, out RectInt overlap)
    {
        overlap = new RectInt();

        if (!rect.Overlaps(other))
            return false;

        int minX = Math.Max(rect.min.x, other.min.x);
        int maxX = Math.Min(rect.max.x, other.max.x);

        int minY = Math.Max(rect.min.y, other.min.y);
        int maxY = Math.Min(rect.max.y, other.max.y);

        Vector2Int min = new Vector2Int(minX, minY);
        Vector2Int max = new Vector2Int(maxX, maxY);

        overlap.min = min;
        overlap.max = max;
        return true;
    }

    /// <summary>
    /// Returns whether or not the two rectangles overlap and their overlapping area.
    /// </summary>
    public static bool Overlaps(this Rect rect, Rect other, out Rect overlap)
    {
        overlap = new Rect();

        if (!rect.Overlaps(other))
            return false;

        float minX = Math.Max(rect.min.x, other.min.x);
        float maxX = Math.Min(rect.max.x, other.max.x);

        float minY = Math.Max(rect.min.y, other.min.y);
        float maxY = Math.Min(rect.max.y, other.max.y);

        Vector2 min = new Vector2(minX, minY);
        Vector2 max = new Vector2(maxX, maxY);

        overlap.min = min;
        overlap.max = max;
        return true;
    }

    /// <summary>
    /// Returns whether or not this rectangle fully contains the provided rectangle
    /// </summary>
    public static bool Contains(this Rect rect, Rect other)
    {
        Rect overlap;
        return rect.Overlaps(other, out overlap) && overlap.size == other.size;
    }

    /// <summary>
    /// Returns the y coordinate for this RectInt's top side (y + height)
    /// </summary>
    public static int Top(this RectInt rect)
    {
        return rect.y + rect.height;
    }

    /// <summary>
    /// Returns the x coordinate for this RectInt's right side (x + width)
    /// </summary>
    public static int Right(this RectInt rect)
    {
        return rect.x + rect.width;
    }

    /// <summary>
    /// Enables interactions with this Button and triggers its "Normal" animation
    /// </summary>
    public static void Enable(this Button button)
    {
        button.interactable = true;
        button.animator.SetTrigger("Normal");
    }

    /// <summary>
    /// Disables interactions with this Button and triggers its "Disabled" animation
    /// </summary>
    public static void Disable(this Button button)
    {
        button.interactable = false;
        button.animator.SetTrigger("Disabled");
    }

    /// <summary>
    /// Enables or disables this Button
    /// </summary>
    public static void SetEnabled(this Button button, bool enable)
    {
        if (enable)
            button.Enable();
        else
            button.Disable();
    }

    /// <summary>
    /// Returns a random item from the list.
    /// </summary>
    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Returns a Rect representing the XZ plane (bottom and top) of this Bounds
    /// </summary>
    public static Rect XZPlane(this Bounds bounds)
    {
        return new Rect(bounds.min.x, bounds.min.z, bounds.size.x, bounds.size.z);
    }

    /// <summary>
    /// Returns the area of this rectangle
    /// </summary>
    public static float Area(this Rect rect)
    {
        return rect.width * rect.height;
    }

    /// <summary>
    /// Returns the area of this rectangle
    /// </summary>
    public static int Area(this RectInt rect)
    {
        return rect.width * rect.height;
    }

    /// <summary>
    /// Adds the content of the provided List to this List
    /// </summary>
    public static List<T> Add<T>(this List<T> list, List<T> other)
    {
        foreach (T item in other)
            list.Add(item);

        return list;
    }

    /// <summary>
    /// Returns a new List containing the content of this and the provided List
    /// </summary>
    public static List<T> And<T>(this List<T> list, List<T> other)
    {
        List<T> joinedList = new List<T>(list);

        foreach (T item in other)
            joinedList.Add(item);

        return joinedList;
    }

    /// <summary>
    /// Returns the X and Z components of this Vector3 as a Vector2
    /// </summary>
    public static Vector2 XZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}

using UnityEngine;

public static class FunctionsEffect {
    // Función EaseIn
    public static float EaseIn(float t)
    {
        return t * t;  // t^2
    }

    // Función EaseOut
    public static float EaseOut(float t)
    {
        return 1 - (1 - t) * (1 - t);  // 1 - (1 - t)^2
    }

    // Función EaseInOut
    public static float EaseInOut(float t)
    {
        return 0.5f * (1 - Mathf.Cos(Mathf.PI * t));  // 0.5 * (1 - cos(π * t))
    }
}
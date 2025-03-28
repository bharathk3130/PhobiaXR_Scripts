using System;
using Clickbait.Utilities;

public static class LightCounter
{
    public static Observer<int> LightCount { get; private set; } = new(0);
    public static event Action OnComplete = delegate { };

    public const int MaxLightCount = 10;
    
    public static void Reset()
    {
        LightCount = new Observer<int>(0);
        OnComplete = delegate { };
    }

    public static void IncrementLightCount(int increment)
    {
        LightCount.Value += increment;
        if (LightCount.Value >= MaxLightCount)
        {
            OnComplete.Invoke();
        }
    }
}

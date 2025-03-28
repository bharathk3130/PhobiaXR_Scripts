using Clickbait.Utilities;

public static class PanicHandler
{
    public static readonly Observer<bool> IsPanicking = new(false);

    static int _panicThreshold = 120;

    public static void Initialize()
    {
        HeartRateManager.HeartRate.AddListener(OnHeartRateChange);
    }

    static void OnHeartRateChange(int newHeartRate)
    {
        IsPanicking.Value = newHeartRate > _panicThreshold;
    }
}
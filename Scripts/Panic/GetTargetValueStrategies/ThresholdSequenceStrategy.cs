[System.Serializable]
public class ThresholdSequenceStrategy<T>
{
    public HeartRateValueContainer<T>[] HeartRateValContainer;
    public float PauseDuration = 5;

    T _targetValue;
    
    public T UpdateTargetValueAndReturn(int heartRate)
    {
        _targetValue = GetValue(heartRate);
        return _targetValue;
    }

    
    // Returns to the value corresponding to the heart rate just below the current heart rate
    T GetValue(float heartRate)
    {
        for (int i = 1; i < HeartRateValContainer.Length; i++)
        {
            if (heartRate < HeartRateValContainer[i].HeartRate)
            {
                return HeartRateValContainer[i - 1].Value;
            }
        }

        return HeartRateValContainer[^1].Value;
    }
}

[System.Serializable]
public struct HeartRateValueContainer<T>
{
    public float HeartRate;
    public T Value;
}
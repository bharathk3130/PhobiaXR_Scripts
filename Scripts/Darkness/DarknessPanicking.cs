using UnityEngine;

public class DarknessPanicking : MonoBehaviour
{
    public float minHeartRate = 60f;  // Resting heart rate
    public float maxHeartRate = 120f; // Panic heart rate
    public float minIntensity = -5f;  // Darkest setting
    public float maxIntensity = 2f;   // Brightest setting
    public float smoothSpeed = 2f;    // Speed of transition

    private Color baseAmbientColor;   // Stores the original HDR color
    private float targetIntensity;    // Target intensity to reach
    private float currentIntensity;   // Tracks the current intensity

    void Start()
    {
        // Save the original ambient light color
        baseAmbientColor = RenderSettings.ambientLight;

        // Initialize current intensity based on ambient light
        currentIntensity = Mathf.Log(RenderSettings.ambientLight.maxColorComponent, 2);
    }

    void Update()
    {
        // Get heart rate value from HeartRateManager
        float heartRate = HeartRateManager.HeartRate.Value;

        // Map heart rate to intensity (normalized)
        targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.InverseLerp(minHeartRate, maxHeartRate, heartRate));

        // Smooth transition using Lerp
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * smoothSpeed);

        // Apply smooth intensity to ambient light
        RenderSettings.ambientLight = baseAmbientColor * Mathf.Pow(2, currentIntensity);
    }
}

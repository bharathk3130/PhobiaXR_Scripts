using TMPro;
using UnityEngine;

public class HeartRateUI : MonoBehaviour
{
    [SerializeField] HeartRateProvider _heartRateProvider;
    [SerializeField] TextMeshProUGUI _heartRateText;
    
    void Awake()
    {
        if (_heartRateProvider.DetectingHeartRate.Value)
        {
            UpdateHeartRateUI(HeartRateManager.HeartRate.Value);
        } else
        {
            _heartRateText.text = "-";
        }

        _heartRateProvider.DetectingHeartRate.AddListener(OnDetectingHeartRateChange);
        HeartRateManager.HeartRate.AddListener(UpdateHeartRateUI);
    }

    void OnDetectingHeartRateChange(bool detectingHeartRate)
    {
        if (detectingHeartRate)
        {
            UpdateHeartRateUI(HeartRateManager.HeartRate.Value);
        } else
        {
            _heartRateText.text = "-";
        }
    }

    void UpdateHeartRateUI(int newHeartRate) => _heartRateText.text = newHeartRate.ToString();
}

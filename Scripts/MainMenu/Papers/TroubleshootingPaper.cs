using UnityEngine;

public class TroubleshootingPaper : MonoBehaviour
{
    [SerializeField] HeartRateProvider _heartRateProvider;
    [SerializeField] GameObject _cantFindFileText;
    [SerializeField] GameObject _cantDetectHeartRateText;
    [SerializeField] GameObject _successText;

    void Start()
    {
        OnFoundFileChange(_heartRateProvider.FoundFile.Value);
        OnDetectingHeartRateChange(_heartRateProvider.DetectingHeartRate.Value);
        
        _heartRateProvider.FoundFile.AddListener(OnFoundFileChange);
        _heartRateProvider.DetectingHeartRate.AddListener(OnDetectingHeartRateChange);
    }

    void OnFoundFileChange(bool foundFile)
    {
        if (foundFile)
        {
            OnDetectingHeartRateChange(_heartRateProvider.DetectingHeartRate.Value);
        } else
        {
            _cantDetectHeartRateText.SetActive(false);
        }

        _cantFindFileText.SetActive(!foundFile);
        _successText.SetActive(false);
    }
    
    void OnDetectingHeartRateChange(bool detectingHeartRate)
    {
        if (detectingHeartRate)
        {
            _cantDetectHeartRateText.SetActive(false);
            _successText.SetActive(true);
        }
        else
        {
            if (_heartRateProvider.FoundFile.Value)
            {
                _cantDetectHeartRateText.SetActive(true);
            }
            
            _successText.SetActive(false);
        }
    }
}
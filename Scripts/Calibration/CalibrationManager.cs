using UnityEngine;
using UnityEngine.UI;

public class CalibrationManager : MonoBehaviour
{
    [SerializeField] CalibrationSFXManager _calibrationSFXManager;
    [SerializeField] GameObject _calibrationUI;
    [SerializeField] GameObject _lever;
    [SerializeField] Button _enableCalibrationButton;
    [SerializeField] Button _useHeadsetHeightButton;
    [SerializeField] HeightSetter _heightSetter;
    
    void Start()
    {
        SetCalibrationVisualsActive(false);
        SetAutomaticVisualsActive(true);
        
        _enableCalibrationButton.onClick.AddListener(EnableCalibration);
        _useHeadsetHeightButton.onClick.AddListener(UseHeadsetHeight);
    }

    void EnableCalibration()
    {
        _calibrationSFXManager.PlayButtonClickSFX();
        SetCalibrationVisualsActive(true);
        SetAutomaticVisualsActive(false);
        
        _heightSetter.EnableManualCalibration();
    }
    
    void UseHeadsetHeight()
    {
        _calibrationSFXManager.PlayButtonClickSFX();
        SetCalibrationVisualsActive(false);
        SetAutomaticVisualsActive(true);
        
        _heightSetter.EnableAutomaticCalibration();
    }

    void SetCalibrationVisualsActive(bool active)
    {
        _calibrationUI.SetActive(active);
        _useHeadsetHeightButton.gameObject.SetActive(active);
        _lever.SetActive(active);
    }

    void SetAutomaticVisualsActive(bool active)
    {
        _enableCalibrationButton.gameObject.SetActive(active);
    }
}

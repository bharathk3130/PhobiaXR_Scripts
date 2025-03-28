using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : ValidatedMonoBehaviour
{
    [SerializeField, Scene] HeightSetter _heightSetter;
    [SerializeField, Child] Slider _slider;
    [SerializeField] LeverCalibration _lever;
    [SerializeField] CalibrationSFXManager _calibrationSFXManager;
    [SerializeField] float _maxHeight = 2;
    [SerializeField] float _maxSpeed = 5;

    [SerializeField] float _standingHeight = 1.62f;

    float _speedMultiplier;
    float _speed;

    float _height;
    
    void Start()
    {
        SetSpeed();
        SetHeight(_standingHeight);
    }
    
    void Update()
    {
        if (_speed != 0)
        {
            IncrementHeight(_speed * Time.deltaTime);
        }
    }

    void SetHeight(float val)
    {
        _height = Mathf.Clamp(val, 0, _maxHeight);
        _slider.value = _height;
        _heightSetter.SetCameraYOffset(_height);
        
        if (!_lever.AlreadyStoppedByCalibration)
        {
            if (_height == 0 || Mathf.Approximately(_height, _maxHeight))
            {
                _calibrationSFXManager.StopLeverMovingSFX();
                _calibrationSFXManager.PlayLeverStopSFX();
                _lever.AlreadyStoppedByCalibration = true;
            }
        } else
        {
            // Lever was stopped by calibration and not let go yet and has started moving again
            if (_height != 0 && !Mathf.Approximately(_height, _maxHeight))
            {
                _calibrationSFXManager.PlayLeverMovingSFX();
                _lever.AlreadyStoppedByCalibration = false;
            }
        }
    }

    void IncrementHeight(float increment)
    {
        if ((increment > 0 && _height < _maxHeight) || (increment < 0) && _height > 0)
        {
            SetHeight(_height + increment);
        }
    }

    public void SetSpeedMultiplier(float speedMultiplier)
    {
        _speedMultiplier = speedMultiplier;
        SetSpeed();
    }

    void SetSpeed() => _speed = _maxSpeed * _speedMultiplier;
}
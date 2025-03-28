using System.Collections;
using Clickbait.Utilities;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverCalibration : ValidatedMonoBehaviour
{
    [SerializeField, Self] XRLever _lever;
    [SerializeField, Scene] Calibration _calibration;
    [SerializeField] CalibrationSFXManager _calibrationSFXManager;
    [SerializeField] Transform _leverHandle;

    [SerializeField] float _leverRange = 50;
    [SerializeField] float _resetDuration = 0.1f; 
    
    [SerializeField, NonEditable, Tooltip("Pulling back: -1 to 0, NeutralL 0, Pushing forward: 0 to 1")]
    float _leverPercent;

    public bool IsGrabbed { get; private set; }
    public float LeverPercent => _leverPercent;
    
    HapticController _hapticController;
    [SerializeField, NonEditable] HapticController.VibrationHand _currentHand;

    public bool AlreadyStoppedByCalibration;
    
    // lever range: 50 to 0, 360 to 310 degrees --> 50 = increase, 310 = decrease

    void Start()
    {
        _hapticController = HapticController.Instance;
        
        _lever.selectEntered.AddListener(OnLeverGrab);
        _lever.selectExited.AddListener(_ => OnLeverLetGo());
        
        StartCoroutine(ResetLever());
    }

    void Update()
    {
        if (IsGrabbed)
        {
            SetLeverPercent();
        }
    }
    
    void SetLeverPercent()
    {
        float angle = _leverHandle.localEulerAngles.x;

        if (angle > 0 && angle <= _leverRange) // Increase range (0 to 50)
            SetLeverPercent(Mathf.InverseLerp(0, _leverRange, angle)); // 0 to 1 (Increase)
        else if (angle < 360 || angle >= 360 - _leverRange) // Decrease range (310 to 359)
            SetLeverPercent(-Mathf.InverseLerp(360, 360 - _leverRange, angle)); // 0 to -1 (Decrease)
        else
            SetLeverPercent(0); // Neutral position at 0 degrees
    }

    void OnLeverGrab(SelectEnterEventArgs args)
    {
        IsGrabbed = true;
        
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            if (controllerInteractor.CompareTag("RightController"))
            {
                _currentHand = HapticController.VibrationHand.Right;
            } else if (controllerInteractor.CompareTag("LeftController"))
            {
                _currentHand = HapticController.VibrationHand.Left;
            }
        }
        
        _calibrationSFXManager.PlayLeverMovingSFX();
        _hapticController.Vibrate(VibrationDataContainer.LeverVD, _currentHand);
    }
    
    void OnLeverLetGo()
    {
        IsGrabbed = false;
        
        SetLeverPercent(0);
        StartCoroutine(ResetLever());

        if (AlreadyStoppedByCalibration)
        {
            AlreadyStoppedByCalibration = false;
        } else
        {
            _calibrationSFXManager.StopLeverMovingSFX();
            _calibrationSFXManager.PlayLeverStopSFX();
        }

        _hapticController.Vibrate(VibrationDataContainer.NoVibrationVD, _currentHand);
    }

    IEnumerator ResetLever()
    {
        float startAngle = _lever.GetHandleAngle();
        float target = startAngle > 180 ? 360 : 0;
        float elapsedTime = 0;

        while (!IsGrabbed && elapsedTime < _resetDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _resetDuration;
            _lever.SetHandleAngle(Mathf.Lerp(startAngle, target, t));
            yield return null;
        }

        _lever.SetHandleAngle(0);
    }

    void SetLeverPercent(float percent)
    {
        _leverPercent = percent;
        _calibration.SetSpeedMultiplier(_leverPercent);
    }
}
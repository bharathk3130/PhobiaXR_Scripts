using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

// HR = Heart Rate. Handles the breathing manager, panic manager and the keyboard controlled biometrics
public class HRManagersHandler : ValidatedMonoBehaviour
{
    [SerializeField, Child] HeartRateProvider _heartRateProvider;
    [SerializeField, Child] KeyboardControlledBiometric _keyboardBiometrics;

    static HeartRateInputType s_heartRateInputType = HeartRateInputType.Biometric;
    static bool s_setUpPanicHandler = true;

    public event Action OnInputTypeChange = delegate { };
    
    void Awake()
    {
        if (s_heartRateInputType == HeartRateInputType.Biometric)
        {
            EnableBiometricInput();
        } else
        {
            EnableKeyboardInput();
        }
        
        if (s_setUpPanicHandler)
        {
            s_setUpPanicHandler = false;
            PanicHandler.Initialize();
        }
    }
    
    public void UseKeyboardInput()
    {
        if (s_heartRateInputType != HeartRateInputType.Keyboard)
        {
            OnInputTypeChange.Invoke();
            s_heartRateInputType = HeartRateInputType.Keyboard;
            EnableKeyboardInput();
        }
    }

    public void UseBiometricInput()
    {
        if (s_heartRateInputType != HeartRateInputType.Biometric)
        {
            OnInputTypeChange.Invoke();
            s_heartRateInputType = HeartRateInputType.Biometric;
            EnableBiometricInput();
        }
    }

    void EnableBiometricInput()
    {
        _heartRateProvider.gameObject.SetActive(true);
        _keyboardBiometrics.gameObject.SetActive(false);
    }

    void EnableKeyboardInput()
    {
        _heartRateProvider.gameObject.SetActive(false);
        _keyboardBiometrics.gameObject.SetActive(true);
    }
}

public enum HeartRateInputType
{
    Biometric,
    Keyboard
}

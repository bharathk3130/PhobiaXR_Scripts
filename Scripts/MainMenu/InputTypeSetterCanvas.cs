using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputTypeSetterCanvas : MonoBehaviour
{
    [Header("References")]
    [SerializeField] HRManagersHandler _hrManagersHandler;
    [SerializeField] MainMenuSFXManager _mainMenuSFXManager;
    [SerializeField] WatchPedestal _watchPedestal;
    [SerializeField] Sprite _roundedButtonSprite;
    [SerializeField] Sprite _hollowRoundedButtonSprite;
    
    [Header("Biometric")]
    [SerializeField] Transform _biometricParent;
    [SerializeField] Button _biometricButton;
    [SerializeField] Image _biometricImage;
    [SerializeField] TextMeshProUGUI _biometricText;
    float _biometricParentScale;
    
    [Header("Manual")]
    [SerializeField] Transform _manualParent;
    [SerializeField] Button _manualButton;
    [SerializeField] Image _manualImage;
    [SerializeField] TextMeshProUGUI _manualText;
    float _manualParentSize;

    [Header("Settings")]
    [SerializeField] float _smallButtonScale = 0.75f;
    [SerializeField] float _parentScaleDuration = 0.2f;

    public HeartRateInputType CurrentHeartRateInputType { get; private set; }

    void Start()
    {
        _biometricButton.onClick.AddListener(EnableBiometric);
        _manualButton.onClick.AddListener(EnableManual);
    }

    void EnableBiometric()
    {
        CurrentHeartRateInputType = HeartRateInputType.Biometric;
        EnableBiometricVisuals();
        EnableBiometricsLogic();
    }

    void EnableManual()
    {
        CurrentHeartRateInputType = HeartRateInputType.Keyboard;
        EnableManualVisuals();
        EnableManualLogic();
    }

    void EnableBiometricVisuals()
    {
        _biometricParentScale = 1;
        _manualParentSize = _smallButtonScale;
        StartCoroutine(ChangeScale());
        
        _biometricImage.sprite = _roundedButtonSprite;
        _manualImage.sprite = _hollowRoundedButtonSprite;

        _biometricText.color = Color.black;
        _manualText.color = Color.white;
    }

    void EnableBiometricsLogic()
    {
        _hrManagersHandler.UseBiometricInput();
        _mainMenuSFXManager.PlayButtonClickSFX(transform.position);
    }

    void EnableManualVisuals()
    {
        _biometricParentScale = _smallButtonScale;
        _manualParentSize = 1;
        StartCoroutine(ChangeScale());
        
        _biometricImage.sprite = _hollowRoundedButtonSprite;
        _manualImage.sprite = _roundedButtonSprite;
        
        _biometricText.color = Color.white;
        _manualText.color = Color.black;
    }

    void EnableManualLogic()
    {
        _hrManagersHandler.UseKeyboardInput();
        _mainMenuSFXManager.PlayButtonClickSFX(transform.position);
        _watchPedestal.Begin();
    }
    
    IEnumerator ChangeScale()
    {
        Vector3 manualParentStartScale = _manualParent.localScale;
        Vector3 biometricParentStartScale = _biometricParent.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < _parentScaleDuration)
        {
            elapsedTime += Time.deltaTime;

            float lerpFactor = elapsedTime / _parentScaleDuration;

            // Lerp the scales of the parents
            _manualParent.localScale = Vector3.Lerp(manualParentStartScale, new Vector3(_manualParentSize, _manualParentSize, _manualParentSize), lerpFactor);
            _biometricParent.localScale = Vector3.Lerp(biometricParentStartScale, new Vector3(_biometricParentScale, _biometricParentScale, _biometricParentScale), lerpFactor);

            yield return null;
        }

        // Ensure the final scale is exactly the target size
        _manualParent.localScale = new Vector3(_manualParentSize, _manualParentSize, _manualParentSize);
        _biometricParent.localScale = new Vector3(_biometricParentScale, _biometricParentScale, _biometricParentScale);
    }

}

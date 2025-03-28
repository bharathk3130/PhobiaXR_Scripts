using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiftPause : ValidatedMonoBehaviour
{
    [SerializeField, Self] Button _button;
    [SerializeField, Child] TextMeshProUGUI _buttonText;
    [SerializeField, Scene] LiftSizeController _liftSizeController;
    [SerializeField] LiftSFXManager _liftSFXManager;

    bool _isPaused;

    void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (_isPaused)
        {
            UnpauseGame();
        } else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        _buttonText.text = "Unpause";
        _isPaused = true;
        _liftSizeController.enabled = false;
        
        _liftSFXManager.PlayButtonClickSFX();
        _liftSFXManager.StopLiftCreakingSFX();
    }
    
    public void UnpauseGame()
    {
        _buttonText.text = "Pause";
        _isPaused = false;
        _liftSizeController.enabled = true;
        
        _liftSFXManager.PlayButtonClickSFX();
        if (_liftSizeController.IsMoving)
        {
            _liftSFXManager.PlayLiftCreakingSFX();
        }
    }
}
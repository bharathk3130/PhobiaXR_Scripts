using Clickbait.Utilities;
using KBCore.Refs;
using UnityEngine;

public class LiftVictory : ValidatedMonoBehaviour
{
    [SerializeField, Scene] LiftSizeController _liftController;
    [SerializeField, Scene] LiftSFXManager _liftSFXManager;
    [SerializeField] GameObject _victoryUI;
    [SerializeField] float _countdownTime = 20;
    
    CountDownTimer _countdown;

    bool _showingVictoryUI;

    void Awake()
    {
        _countdown = new CountDownTimer(_countdownTime);
        _countdown.OnTimerStop += ShowVictoryUI;
        
        _victoryUI.SetActive(false);
    }

    void ShowVictoryUI()
    {
        _liftSFXManager.PlayCompletedLevelSFX();
        _showingVictoryUI = true;
        _victoryUI.SetActive(true);
    }
    
    void Update()
    {
        if (_showingVictoryUI) return;
        
        if (_countdown.IsRunning)
        {
            _countdown.Tick(Time.deltaTime);
            
            if (_liftController.ScalePercent > 0)
            {
                _countdown.Abort();
            }
        } else
        {
            if (_liftController.ScalePercent == 0)
            {
                _countdown.Start();
            }
        }
    }
}

using KBCore.Refs;
using UnityEngine;

public class HeartRatePaper : ValidatedMonoBehaviour
{
    [SerializeField, Scene] WatchPedestal _watchPedestal;
    [SerializeField, Scene] ChooseScenePaper _chooseScenePaper;
    [SerializeField, Scene] HRManagersHandler _hrManagersHandler;
    [SerializeField, Scene] HeartRateProvider _heartRateProvider;
    [SerializeField] MainMenuSFXManager _mainMenuSFXManager;
    [SerializeField] GameObject _checkboxParent;

    void Awake()
    {
        _checkboxParent.SetActive(false);
        
        _hrManagersHandler.OnInputTypeChange += Reset;
        _heartRateProvider.DetectingHeartRate.AddListener(detecting =>
        {
            if (!detecting)
            {
                Reset();
            }
        });
    }

    void Start()
    {
        HeartRateManager.HeartRate.AddListener(StartedGettingHeartRate);
        if (HeartRateManager.HeartRate.Value > 0)
        {
            StartedGettingHeartRate(HeartRateManager.HeartRate.Value);
        }
    }

    public void Reset()
    {
        _checkboxParent.SetActive(false);
        _chooseScenePaper.DeactiveteButtons();
        HeartRateManager.HeartRate.AddListener(StartedGettingHeartRate);
    }

    void StartedGettingHeartRate(int heartRate)
    {
        HeartRateManager.HeartRate.RemoveListener(StartedGettingHeartRate);
        
        _checkboxParent.SetActive(true);
        _chooseScenePaper.ActivateButtons();
        _mainMenuSFXManager.PlayHeartRateDetectedSFX();
        _watchPedestal.Begin();
    }
}

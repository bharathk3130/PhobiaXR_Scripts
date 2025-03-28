using UnityEngine;

public class DarknessManager : MonoBehaviour
{
    [SerializeField] CompletionUI _completionUI;
    [SerializeField] DarknessSFXManager _darknessSFXManager;

    void Awake()
    {
        LightCounter.Reset();
        LightCounter.OnComplete += EnableCompletionUI;
    }

    void EnableCompletionUI()
    {
        _completionUI.EnableSelf();
        _darknessSFXManager.PlayCompletedLevelSFX();
    }
}

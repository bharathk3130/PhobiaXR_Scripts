using KBCore.Refs;
using TMPro;
using UnityEngine;

public class CompletionUI : ValidatedMonoBehaviour
{
    [SerializeField, Scene] PanicCounter _panicCounter;
    [SerializeField] GameObject _visuals;
    [SerializeField] TextMeshProUGUI _panicCountText;
    [SerializeField] TextMeshProUGUI _suggestionText;

    void Start()
    {
        DisableSelf();
    }

    public void EnableSelf()
    {
        _visuals.SetActive(true);
        int panicCount = _panicCounter.PanicCount;

        if (panicCount == 1)
        {
            _panicCountText.text = "Panicked 1 time";
            _suggestionText.text = "You're getting there! Take a break and try again later";
        } else
        {
            _panicCountText.text = $"Panicked {panicCount} times";

            if (panicCount >= 2)
            {
                _suggestionText.text = "Take a break and try again later";
            } else
            {
                _suggestionText.text = "You seem to have overcome your fear!";
            }
        }
    }

    void DisableSelf()
    {
        _visuals.SetActive(false);
    }
}

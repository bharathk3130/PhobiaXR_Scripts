using TMPro;
using UnityEngine;

public class LightCounterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _lightCounterText;

    void Start()
    {
        UpdateCounterText(0);
        LightCounter.LightCount.AddListener(UpdateCounterText);
    }

    void UpdateCounterText(int count)
    {
        _lightCounterText.text = $"Lights on: {count}/{LightCounter.MaxLightCount}";
    }
}

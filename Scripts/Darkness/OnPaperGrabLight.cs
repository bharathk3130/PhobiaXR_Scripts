using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnPaperGrabLight : MonoBehaviour
{
    [SerializeField] Light _light;
    public float oldIntensity, newIntensity;
    
    public void OnGrab(SelectEnterEventArgs args)
    {
        _light.intensity = newIntensity;
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        _light.intensity = oldIntensity;
    }
}

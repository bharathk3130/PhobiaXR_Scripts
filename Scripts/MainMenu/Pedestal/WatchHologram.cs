using KBCore.Refs;
using UnityEngine;

public class WatchHologram : ValidatedMonoBehaviour
{
    [SerializeField, Scene] MainMenuSFXManager _mainMenuSFXManager;
    [SerializeField] GameObject _visuals;
    [SerializeField] WatchPedestal _watchPedestal;

    public void OnGrab()
    {
        _visuals.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<RotateSelf>().enabled = false;
        
        _mainMenuSFXManager.PlayEquippedWatchSFX();
        _watchPedestal.OnWatchGrabbed();
    }
}

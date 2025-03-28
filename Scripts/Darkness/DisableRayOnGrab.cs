using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableRayOnGrab : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable.selectEntered.AddListener(DisableRay);
        grabInteractable.selectExited.AddListener(args => {
            if (!args.isCanceled)
            {
                EnableRay(args.interactorObject);
            }
        });
    }

    void DisableRay(SelectEnterEventArgs args)
    {
        rayInteractor.enabled = false; // Disable ray when grabbing
    }

    void EnableRay(IXRSelectInteractor interactor)
    {
        rayInteractor.enabled = true; // Enable ray when released
    }
}

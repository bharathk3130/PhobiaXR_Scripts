using KBCore.Refs;
using UnityEngine;

public class MCBTrip : ValidatedMonoBehaviour
{
    [SerializeField, Scene] DarknessSFXManager _darknessSFXManager;
    
    Transform mcb;
    public bool mcbOn;

    void Start()
    {
        mcb = GetComponent<Transform>();
    }
    
    void Update()
    {
        if (mcbOn)
        {
            if (mcb.rotation.eulerAngles.x < 320)
            {
                FlipMCBSwitch();
            }
        } else
        {
            if (mcb.rotation.eulerAngles.x >= 320)
            {
                FlipMCBSwitch();
            }
        }
    }

    void FlipMCBSwitch()
    {
        mcbOn = !mcbOn;
        _darknessSFXManager.PlayMCBTripSFX(transform.position);
    }
}

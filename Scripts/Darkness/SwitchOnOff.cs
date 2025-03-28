using System;
using UnityEngine;

public class SwitchOnOff : MonoBehaviour
{
    [SerializeField] DarknessSFXManager _darknessSFXManager;
    [SerializeField] Transform button;
    [SerializeField] HouseLight _houseLight;

    public event Action OnSwitchFlip = delegate { };
    
    public MCBTrip MCBTrip;
    bool isSwitchOn = false;
    float onAngle = 20f;
    float offAngle = -20f;

    bool isLightOn;

    void Update()
    {
        if (isLightOn)
        {
            if (!isSwitchOn || !MCBTrip.mcbOn)
            {
                _houseLight.SwitchOffLight();
                isLightOn = false;
            }
        } else
        {
            if (isSwitchOn && MCBTrip.mcbOn)
            {
                _houseLight.SwitchOnLight();
                isLightOn = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            float targetAngle = isSwitchOn ? offAngle : onAngle;
            isSwitchOn = !isSwitchOn;
            _darknessSFXManager.PlaySwitchSFX(transform.position);
            OnSwitchFlip.Invoke();

            Vector3 targetRotation = new Vector3(button.localEulerAngles.x, targetAngle, button.localEulerAngles.z);
            button.localEulerAngles = targetRotation;
        }
    }
}

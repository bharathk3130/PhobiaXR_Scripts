using System.Collections;
using KBCore.Refs;
using UnityEngine;

public class WatchEquipEffect : ValidatedMonoBehaviour
{
    [SerializeField, Child] Light _light;
    [SerializeField, Child] ParticleSystem _particleEffect;
    [SerializeField] float _lightPulseDuration = 0.5f;

    float _initialLightIntensity;

    void Awake()
    {
        _light.gameObject.SetActive(false);
        _initialLightIntensity = _light.intensity;
    }

    public void PlayEffect()
    {
        StartCoroutine(PlayLightPulseEffect());
        _particleEffect.Play();
    }

    IEnumerator PlayLightPulseEffect()
    {
        _light.gameObject.SetActive(true);
        _light.intensity = _initialLightIntensity;
        float timeElapsed = 0;
        
        while (_light.intensity > 0)
        {
            timeElapsed += Time.deltaTime;
            _light.intensity = Mathf.Lerp(_initialLightIntensity, 0, timeElapsed / _lightPulseDuration);
            yield return null;
        }
        
        _light.gameObject.SetActive(false);
    }
}

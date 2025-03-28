using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _calmingMusicAudioSource;
    [SerializeField] float _fadeTime = 3f;
    [SerializeField] float _calmingMusicVolume = 0.1f;
    
    bool _startedCalmingAudio;

    void Start() => PreloadAudio();

    public void StartCalmingMusic()
    {
        _calmingMusicAudioSource.volume = 0f;
        
        if (!_startedCalmingAudio)
        {
            _calmingMusicAudioSource.Play();
            _startedCalmingAudio = true;
        } else
        {
            _calmingMusicAudioSource.UnPause();
        }

        StartCoroutine(FadeMusic(_calmingMusicVolume));
    }

    public void StopCalmingMusic()
    {
        StartCoroutine(FadeMusic(0f));
    }
    
    void PreloadAudio()
    {
        _calmingMusicAudioSource.Play();
        _calmingMusicAudioSource.Pause();
    }

    IEnumerator FadeMusic(float targetVolume)
    {
        float startVolume = _calmingMusicAudioSource.volume;
        float currentTime = 0f;

        while (currentTime < _fadeTime)
        {
            _calmingMusicAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / _fadeTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        _calmingMusicAudioSource.volume = targetVolume;

        if (targetVolume == 0f)
        {
            _calmingMusicAudioSource.Pause();
        }
    }
}
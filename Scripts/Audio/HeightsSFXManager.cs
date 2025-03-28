using UnityEngine;

public class HeightsSFXManager : SFXManagerBase
{
    [SerializeField] AudioClip _equipGunSFX;
    [SerializeField] AudioClip _zombieDeathSFX;
    [SerializeField] AudioSource _shootAudioSource;

    [SerializeField] AudioClip[] _zombieScreams;
    [SerializeField] AudioSource _zombieScreamAudioSource;
    
    public void PlayEquipGunSFX(Vector3 pos) => PlaySFX(_equipGunSFX, pos);
    public void PlayShootSFX() => _shootAudioSource.Play();
    public void PlayZombieScream()
    {
        _zombieScreamAudioSource.clip = _zombieScreams[Random.Range(0, _zombieScreams.Length)];
        _zombieScreamAudioSource.panStereo = Random.Range(-0.75f, 0.75f);
        _zombieScreamAudioSource.Play();
    }

    public void PlayZombieDeathEffect() => PlaySFX(_zombieDeathSFX, volume: 0.25f);
}
using System;
using System.Collections;
using UnityEngine;

public class FadeSceneTransition : MonoBehaviour, ISceneTransition
{
    [SerializeField] GameObject _fadeCanvas;
    [SerializeField] SFXManagerBase _sfxManager;
    [SerializeField] float _fadeAnimationDuration = 0.75f;

    public event Action OnTransitionComplete = delegate { };
    public bool CanLoadNextScene { get; set; }
    
    Animator _fadeAnim;
    ISceneTransition _sceneTransitionImplementation;

    static bool s_fadedIn;
    static readonly int _sceneStartHash = Animator.StringToHash("SceneStart");
    static readonly int _sceneEndHash = Animator.StringToHash("SceneEnd");

    void Awake()
    {
        _fadeAnim = _fadeCanvas.GetComponent<Animator>();
        
        if (s_fadedIn)
        {
            StartCoroutine(PlayStartSceneTransition());
        } else
        {
            _fadeCanvas.SetActive(false);
        }
    }

    IEnumerator PlayStartSceneTransition()
    {
        s_fadedIn = false;
        _fadeCanvas.SetActive(true);
        _fadeAnim.SetTrigger(_sceneStartHash);

        yield return new WaitForSeconds(1);
        _fadeCanvas.SetActive(false);
    }
    
    public void EndScene(Material nextSceneMat)
    {
        StartCoroutine(PlayEndSceneTransition());
    }

    IEnumerator PlayEndSceneTransition()
    {
        s_fadedIn = true;
        
        yield return new WaitForSeconds(0.5f); // Let the button animation play
        _sfxManager.PlaySceneTransitionSFX();
        _fadeCanvas.SetActive(true);
        _fadeAnim.SetTrigger(_sceneEndHash);

        yield return new WaitForSeconds(_fadeAnimationDuration);
        LoadNextScene();
    }

    void LoadNextScene()
    {
        CanLoadNextScene = true;
        OnTransitionComplete.Invoke();
    }

    public void SceneFinishedLoading() { }
}
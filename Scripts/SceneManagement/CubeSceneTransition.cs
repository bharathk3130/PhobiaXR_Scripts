using System;
using System.Collections;
using UnityEngine;

public class CubeSceneTransition : MonoBehaviour, ISceneTransition
{
    [SerializeField] TransitionCube _transitionCube;
    [SerializeField] Camera _mainCam;
    [SerializeField] LayerMask _onlyTransitionCube;
    [SerializeField] GameObject _transitionCamera;
    [SerializeField] SFXManagerBase _sfxManager;
    
    public event Action OnTransitionComplete = delegate { };
    
    public bool CanLoadNextScene { get; set; }

    void Start()
    {
        _transitionCamera.SetActive(false);
    }

    public void EndScene(Material nextSceneMat)
    {
        StartCoroutine(EndSceneCoroutine(nextSceneMat));
    }

    IEnumerator EndSceneCoroutine(Material nextSceneMat)
    {
        // Enable the transition camera for 1 frame to update the render texture
        _transitionCamera.SetActive(true);
        yield return null;
        _transitionCamera.SetActive(false);
        
        // Transition camera causes a lag so wait a bit before starting the transition
        yield return new WaitForSeconds(0.5f);
        
        _sfxManager.PlaySceneTransitionSFX();
        _sfxManager.StartedSceneTransition = true;
        _transitionCube.transform.GetChild(0).gameObject.SetActive(true);
        SetEndSceneCameraSettings();

        _transitionCube.OnTransitionComplete += () =>
        {
            CanLoadNextScene = true;
            OnTransitionComplete.Invoke();
        };
        
        _transitionCube.StartTransition(nextSceneMat);
    }

    public void SceneFinishedLoading()
    {
        _transitionCube.SceneFinishedLoading();
    }

    void SetEndSceneCameraSettings()
    {
        _mainCam.cullingMask = _onlyTransitionCube;
        _mainCam.clearFlags = CameraClearFlags.SolidColor;
    }
}

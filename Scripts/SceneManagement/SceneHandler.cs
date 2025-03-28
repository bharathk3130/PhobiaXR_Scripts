using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    ISceneTransition _sceneTransition;

    void Start()
    {
        _sceneTransition = GetComponent<ISceneTransition>();
    }

    public void ChangeScene(SceneSO sceneSO)
    {
        _sceneTransition.EndScene(sceneSO.SceneMaterial);
        
        StartCoroutine(LoadSceneAsync(sceneSO.Scene));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        if (asyncLoad != null)
        {
            asyncLoad.allowSceneActivation = false; // Load in the background without switching immediately
            
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f) // Scene is almost ready
                {
                    _sceneTransition.SceneFinishedLoading();

                    Action loadNextScene = () => asyncLoad.allowSceneActivation = true;
                    if (_sceneTransition.CanLoadNextScene)
                    {
                        loadNextScene();
                    } else
                    {
                        _sceneTransition.OnTransitionComplete += loadNextScene;
                    }
                    yield break;
                }

                yield return null;
            }
        }
    }
}
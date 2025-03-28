using System;
using UnityEngine;

public interface ISceneTransition
{
    public void EndScene(Material nextSceneMat);
    public void SceneFinishedLoading();
    public event Action OnTransitionComplete;
    public bool CanLoadNextScene { get; set; }
}
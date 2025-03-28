using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSceneButton : ValidatedMonoBehaviour
{
    [SerializeField, Self] Button _button;
    [SerializeField] SFXManagerBase _sfxManager;
    [SerializeField] SceneHandler _sceneHandler;
    [SerializeField] SceneSO _sceneSO;
    
    void Awake()
    {
        _button.onClick.AddListener(ChangeScene);
    }

    void ChangeScene()
    {
        _sfxManager.PlayButtonClickSFX(transform.position);
        _sceneHandler.ChangeScene(_sceneSO);
    }
}

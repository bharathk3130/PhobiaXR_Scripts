using System;
using KBCore.Refs;
using UnityEngine;

public abstract class DemoBase : ValidatedMonoBehaviour
{
    public event Action OnComplete = delegate { };
    
    [SerializeField] protected Animator _controllerAnim;
    [SerializeField] protected GameObject _visualsGO;

    public bool IsCompleted { get; private set; }

    protected virtual void Awake()
    {
        _visualsGO.SetActive(false);
        IsCompleted = GetIsCompleted();
    }

    protected void Completed()
    {
        IsCompleted = true;
        OnComplete.Invoke();
        SaveAsCompleted();
        _visualsGO.SetActive(false);
    }
    
    bool GetIsCompleted() => PlayerPrefsManager.DataExists(GetType().Name);
    void SaveAsCompleted() => PlayerPrefsManager.SaveData(GetType().Name, 1);
    
    [ContextMenu("Clear All Data")]
    void ClearData() => PlayerPrefsManager.ClearAllData();
    
    [ContextMenu("Clear data for this demo")]
    void ClearDemoData() => PlayerPrefsManager.ClearData(GetType().Name);
}
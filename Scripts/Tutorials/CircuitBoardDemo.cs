using System.Collections;
using KBCore.Refs;
using UnityEngine;

public class CircuitBoardDemo : DemoBase
{
    [SerializeField, Scene] Connection _tutorialConnection;
    [SerializeField, Scene] CircuitBoard _circuitBoard;
    [SerializeField, Scene] LiftPause _liftPause;
    [SerializeField] GameObject _teachingTextGO;
    [SerializeField] GameObject _grabbingVisuals;
    [SerializeField] GameObject _draggingVisuals;
    
    bool _isDragging;
    
    protected override void Awake()
    {
        base.Awake();

        if (!IsCompleted)
        {
            StartCoroutine(StartDemo());
        } else
        {
            _tutorialConnection.gameObject.SetActive(false);
            _teachingTextGO.SetActive(false);
        }
    }

    IEnumerator StartDemo()
    {
        yield return null; // Wait for 1 frame before disabling the lift controller by pausing the game so that it initializes first
        _liftPause.PauseGame();
        
        _tutorialConnection.gameObject.SetActive(true);
        _tutorialConnection.SetCircuitBoard(_circuitBoard);
        
        _teachingTextGO.SetActive(true);
        _visualsGO.SetActive(true);
        
        _grabbingVisuals.SetActive(true);
        _draggingVisuals.SetActive(false);
        
        _controllerAnim.Play("RightHandGrab");

        _tutorialConnection.OnConnect += () =>
        {
            _tutorialConnection.gameObject.SetActive(false);
            _teachingTextGO.SetActive(false);
            _liftPause.UnpauseGame();
            Completed();
        };
    }

    public void OnWireEndGrab()
    {
        if (_isDragging) return;

        _isDragging = true;
        _grabbingVisuals.SetActive(false);
        _draggingVisuals.SetActive(true);
    }
}
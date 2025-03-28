using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Clickbait.Utilities;
using KBCore.Refs;

public class TransitionCube : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] FollowTransform _followTransform;
    [SerializeField] Transform _cube;
    [SerializeField] Transform _transitionCubePos;
    
    [Header("Movement Settings")]
    [SerializeField] float _distance;
    [SerializeField] float _initialMovementDuration;
    [SerializeField] float _endDelay;
    [SerializeField] float _endTransitionDuration;
    
    [Header("Rotation Settings")]
    [SerializeField] float _initialSpinSpeed;
    [SerializeField] float _quickSpinSpeed;
    [SerializeField] float _initialSpeedDuration;
    [SerializeField] float _quickSpinDuration;
    [SerializeField] float _forcedEndingSlowSpinDuration = 0.75f;

    [Space]
    [SerializeField, NonEditable] State _currentState;
    
    Material _newSceneMaterial;
    
    bool _isTransitioning = false;
    Tween _rotationTween;
    Tween _movementTween;

    float _changeMatDelay;
    

    bool _isWaitingToLoad;
    public event Action OnWaitingToLoad = delegate { };
    public event Action OnTransitionComplete = delegate { };

    enum State
    {
        InitialSlowSpin,
        SwitchingSceneFastSpin,
        LoadingSlowSpin,
        EndFastSpin,
        GoToCam,
        Done
    }

    void Awake()
    {
        _changeMatDelay = _initialSpeedDuration + _quickSpinDuration / 2;
    }

    public void StartTransition(Material newSceneMat)
    {
        if (_isTransitioning) return;

        _newSceneMaterial = newSceneMat;

        _followTransform.enabled = false;
        
        _isTransitioning = true;
        _cube.gameObject.SetActive(true);
        StartCoroutine(TransitionSequence());
        
        Invoke(nameof(ChangeMat), _changeMatDelay);
    }

    void ChangeMat() => _cube.GetComponent<MeshRenderer>().material = _newSceneMaterial;

    IEnumerator TransitionSequence()
    {
        // Move forward initially
        _currentState = State.InitialSlowSpin;
        Vector3 localTargetPos = Vector3.forward * _distance;
        _movementTween = _cube.DOLocalMove(localTargetPos, _initialMovementDuration).SetEase(Ease.OutQuad);

        // Step 2: Rotate at _initialSpinSpeed for _initialSpeedDuration seconds
        _rotationTween = _cube.DORotate(new Vector3(0, 360, 0), 360 / _initialSpinSpeed, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        yield return new WaitForSeconds(_initialSpeedDuration);
        _rotationTween.Kill();

        _currentState = State.SwitchingSceneFastSpin;
        // Step 3: Rotate at _quickSpinSpeed for _quickSpinDuration seconds
        _rotationTween = _cube.DORotate(new Vector3(0, 360, 0), 360 / _quickSpinSpeed, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        yield return new WaitForSeconds(_quickSpinDuration);
        _rotationTween.Kill();
        
        Invoke(nameof(SetAsWaitingToLoad), _forcedEndingSlowSpinDuration);
        
        _currentState = State.LoadingSlowSpin;
        // Step 4: Continue rotating at _initialSpinSpeed indefinitely
        _rotationTween = _cube.DORotate(new Vector3(0, 360, 0), 360 / _initialSpinSpeed, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    void SetAsWaitingToLoad()
    {
        _isWaitingToLoad = true;
        OnWaitingToLoad.Invoke();
    }

    void EndTransition()
    {
        if (!_isTransitioning) return;
        _isTransitioning = false;
        StopAllCoroutines();
        _rotationTween.Kill();
        StartCoroutine(EndSequence());
    }

    public void SceneFinishedLoading()
    {
        if (_isWaitingToLoad)
        {
            EndTransition();
        } else
        {
            OnWaitingToLoad += EndTransition;
        }
    }

    IEnumerator EndSequence()
    {
        yield return LineUpWithCam();
        yield return new WaitForSeconds(_endDelay);

        _currentState = State.GoToCam;
        // Step 6: Move and rotate to match _transitionCubePos
        _cube.DOMove(_transitionCubePos.position, _endTransitionDuration).SetEase(Ease.InOutQuad);
        _cube.DORotate(_transitionCubePos.rotation.eulerAngles, _endTransitionDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(_endTransitionDuration);

        _currentState = State.Done;
        OnTransitionComplete.Invoke();
    }

    IEnumerator LineUpWithCam()
    {
        Quaternion targetRotation = _transitionCubePos.rotation;
        while (Quaternion.Angle(_cube.rotation, targetRotation) > 0.1f)
        {
            _cube.rotation = Quaternion.RotateTowards(_cube.rotation, targetRotation, _initialSpinSpeed * Time.deltaTime);
            yield return null;
        }
        _cube.rotation = targetRotation;
    }
}

using System.Collections;
using KBCore.Refs;
using UnityEngine;

public class GroundCutoutShaderHandler : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Scene] Camera _cam;
    [SerializeField, Scene] MainMenuSFXManager _mainMenuSFXManager;
    [SerializeField] Transform _targetObject;
    [SerializeField] Material _groundSeeThroughMat;
    [SerializeField] Transform _holePos;

    [Header("Settings")]
    [SerializeField] float _maxRadius = 0.05f;
    [SerializeField] float _smoothness = 0.01f;
    [SerializeField] float _openingTime = 0.3f;
    
    static readonly int _cutoutPos = Shader.PropertyToID("_CutoutPosition");
    static readonly int _cutoutSize = Shader.PropertyToID("_CutoutSize");
    static readonly int _falloffSize = Shader.PropertyToID("_FalloffSize");

    float _radius;

    void Awake()
    {
        HideHole();
    }

    public void CreateOpening()
    {
        StopCoroutine(nameof(LerpRadius));
        StartCoroutine(LerpRadius(_maxRadius));
        
        _mainMenuSFXManager.PlayPedestalHoleOpeningSFX(_holePos.position);
    }

    public void CloseOpening()
    {
        StopCoroutine(nameof(LerpRadius));
        StartCoroutine(LerpRadius(0));
        
        _mainMenuSFXManager.PlayPedestalHoleClosingSFX(_holePos.position);
    }

    IEnumerator LerpRadius(float target)
    {
        float elapsedTime = 0;
        float startRadius = _radius;

        while (elapsedTime < _openingTime)
        {
            _radius = Mathf.Lerp(startRadius, target, elapsedTime / _openingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _radius = target;
    }

    void Update()
    {
        if (_radius > 0)
        {
            Vector2 cutoutPos = _cam.WorldToViewportPoint(_targetObject.position);
            cutoutPos.y /= Screen.width / Screen.height;

            _groundSeeThroughMat.SetVector(_cutoutPos, cutoutPos);
            _groundSeeThroughMat.SetFloat(_cutoutSize, _radius);
            _groundSeeThroughMat.SetFloat(_falloffSize, _smoothness);
        }
    }

    void OnApplicationQuit() => HideHole();
    
    void HideHole() => _groundSeeThroughMat.SetFloat(_cutoutSize, 0);
}

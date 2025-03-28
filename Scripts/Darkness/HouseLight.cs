using UnityEngine;

public class HouseLight : MonoBehaviour
{
    [SerializeField] GameObject _light;
    [SerializeField] Material _lightOnMaterial;

    Material[] _originalMaterials;
    Material[] _lightOnMaterialArray;
    MeshRenderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _originalMaterials = _meshRenderer.materials;
        _lightOnMaterialArray = new Material[] { _lightOnMaterial };
        
        _light.SetActive(false);
    }

    public void SwitchOnLight()
    {
        _light.SetActive(true);
        _meshRenderer.materials = _lightOnMaterialArray;
        LightCounter.IncrementLightCount(1);
    }

    public void SwitchOffLight()
    {
        _light.SetActive(false);
        _meshRenderer.materials = _originalMaterials;
        LightCounter.IncrementLightCount(-1);
    }
}

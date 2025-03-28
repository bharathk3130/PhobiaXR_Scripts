using Clickbait.Utilities;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Create SceneSO", fileName = "SceneSO", order = 0)]
public class SceneSO : ScriptableObject
{
    public SceneField Scene;
    public Material SceneMaterial;
}
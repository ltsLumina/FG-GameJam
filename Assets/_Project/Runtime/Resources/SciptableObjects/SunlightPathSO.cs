using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Sunlight Path")]
public class SunlightPathSO : ScriptableObject
{
    [SerializeField] string LevelName;

    [SerializeField] List<Vector2> KeyPositions = new List<Vector2>();

    private void OnValidate()
    {
        
    }



}

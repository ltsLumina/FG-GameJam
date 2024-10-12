using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Sunlight Path")]
public class SunlightPathSO : ScriptableObject
{
    [SerializeField] string LevelName;

    [SerializeField] List<Vector3> KeyPositions = new List<Vector3>();

    [Header("Time Between Points in seconds")]
    [SerializeField] List<float> JourneyTimes = new List<float>(); 


    private void OnValidate()
    {
        int properTimeCount = KeyPositions.Count - 1;
        if (JourneyTimes.Count != properTimeCount)
        {
            JourneyTimes.Clear();
        }
        while (JourneyTimes.Count < properTimeCount)
        {
            JourneyTimes.Add(1f);
        }
    }

}

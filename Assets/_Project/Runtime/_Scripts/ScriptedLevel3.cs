#region
using UnityEngine;
#endregion

public class ScriptedLevel3 : MonoBehaviour
{
    [SerializeField] GameObject spider2;

    void Enable()
    {
        spider2.GetComponent<SunAnimation>().enabled = true;
        spider2.SetActive(true);
    }
}

#region
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

public class PlayerCheckEnd : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("the player should win here");
            other.gameObject.GetComponent<Player>().enabled = false;
            other.gameObject.GetComponentInChildren<PlayerInput>().enabled = false;
        }
    }
}

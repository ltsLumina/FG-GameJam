using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("the player should win here");
            other.gameObject.GetComponent<Player>().enabled = false;
            other.gameObject.GetComponent<BetterJumping>().enabled = false;
        }
    }
}

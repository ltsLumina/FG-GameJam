using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Killplane : MonoBehaviour
{
    [SerializeField] float killHeight = -10;

    Transform player;
    
    void Update()
    {
        if (!player) player = FindObjectOfType<Player>().transform;
        
        if (player.position.y < killHeight)
        {
            if (Application.isPlaying)
            {
                Logger.LogWarning("Player has fallen below the kill plane. \nRespawning player at spawn point.");
                player.position = GameManager.Instance.SpawnPoint;
            }
            else if (Application.isEditor)
            {
                player.position = GameManager.Instance.SpawnPoint;
            }
        }
    }
}

#region
using UnityEngine;
#endregion

[ExecuteAlways]
public class Killplane : MonoBehaviour
{
    [SerializeField] float killHeight = -10;

    Player player;

    void Update()
    {
        if (!player) player = FindObjectOfType<Player>();

        if (player.transform.position.y < killHeight)
        {
            if (Application.isPlaying)
            {
                Logger.LogWarning("Player has fallen below the kill plane. \nRespawning player at spawn point.");
                player.transform.position = player.SpawnPoint;
            }
            else if (Application.isEditor) { player.transform.position = player.SpawnPoint; }
        }
    }
}

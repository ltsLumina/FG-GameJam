#region
using UnityEditor;
using UnityEngine;
#endregion

[ExecuteAlways]
public class Killplane : MonoBehaviour
{
    static int deaths;
    [SerializeField] float killHeight = -10;

    Player player;

    bool playerCountedAsDead;
    bool testingMode;

    void Update()
    {
        if (!player) player = FindObjectOfType<Player>();

        if (player.transform.position.y < killHeight)
        {
            if (!playerCountedAsDead)
            {
                playerCountedAsDead = true;

                if (Application.isPlaying)
                {
                    if (testingMode)
                    {
                        player.transform.position = player.SpawnPoint;
                        return;
                    }

                    if (Application.isEditor)
                    {
                        deaths++;

#if UNITY_EDITOR
                        if (deaths == 3 && !testingMode)
                        {
                            if (EditorUtility.DisplayDialog("Death Limit", "Enter Testing Mode?", "Yes", "No"))
                            {
                                testingMode = true;
                                player.transform.position = player.SpawnPoint;
                            }
                            else
                            {
                                deaths = 0;
                                Logger.LogWarning("Player has fallen below the kill plane. \nRespawning player at spawn point.");
                                player.Death();
                            }

                            return;
                        }
#endif
                    }

                    Logger.LogWarning("Player has fallen below the kill plane. \nRespawning player at spawn point.");
                    player.Death();
                }
                else
                {
                    Logger.LogWarning("Player has fallen below the kill plane. \nRespawning player at spawn point.");
                    player.transform.position = player.SpawnPoint;
                }
            }
        }
        else { playerCountedAsDead = false; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new (-1000, killHeight, 0), new (1000, killHeight, 0));
    }
}

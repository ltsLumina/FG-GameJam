using UnityEngine;
using VInspector;

public class GameManager : SingletonPersistent<GameManager>
{
    #region Pause/Resume
    public static bool IsPaused { get; private set; }

    public static void PauseGame()
    {
        IsPaused       = true;
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        IsPaused       = false;
        Time.timeScale = 1;
    }

    public static void TogglePause()
    {
        if (IsPaused) ResumeGame();
        else PauseGame();
    }
    #endregion

    [SerializeField] Vector2 spawnPoint;

    public Vector2 SpawnPoint => spawnPoint;

    void Start()
    {
        if (!Application.isPlaying) PlayerSpawnPoint();
    }

    [Button]
    void PlayerSpawnPoint()
    {
        Debug.Assert(spawnPoint != Vector2.zero, "Spawn point is not set.");
        
        var player = FindObjectOfType<Player>();
        player.transform.position = spawnPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint, 0.5f);
    }
}

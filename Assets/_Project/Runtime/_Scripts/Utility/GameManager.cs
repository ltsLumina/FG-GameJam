using UnityEngine;

public class GameManager : SingletonPersistent<GameManager>
{
    public static bool IsPaused { get; private set; }

    public static void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1;
    }

    public static void TogglePause()
    {
        if (IsPaused) ResumeGame();
        else PauseGame();
    }
}

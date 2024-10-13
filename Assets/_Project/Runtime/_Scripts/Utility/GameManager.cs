#region
using MelenitasDev.SoundsGood;
using UnityEngine;
#endregion

public class GameManager : SingletonPersistent<GameManager>
{
    void Start()
    {
        var intro = new Music(Track.intro);
        var music = new Music(Track.music);
        intro.SetOutput(Output.Music);
        music.SetOutput(Output.Music);

        intro.Play();
        intro.OnComplete(() => music.Play());
    }
    #region Pause/Resume
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
    #endregion
}

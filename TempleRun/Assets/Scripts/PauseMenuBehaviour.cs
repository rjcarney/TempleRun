using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehaviour : MainMenuBehaviour
{
    public static bool paused;

    [Tooltip("Reference to the pause menu object to turn on/off")]
    public GameObject pauseMenu;

    /// <summary>
    /// Reloads our current level, "restarting the game
    /// </summary>
    /// <param name="levelName">The name of the level we want to load</param>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // <summary>
    /// Will turn our pause menu on or off
    /// </summary>
    /// <param name="isPaused"></param>
    public void SetPauseMenu(bool isPaused)
    {
        paused = isPaused;

        Time.timeScale = (paused) ? 0 : 1;
        pauseMenu.SetActive(paused);
    }

    public void Start()
    {
        base.Start();

        if (!UnityAdController.showAds)
        {
            SetPauseMenu(false);
        }
    }
}

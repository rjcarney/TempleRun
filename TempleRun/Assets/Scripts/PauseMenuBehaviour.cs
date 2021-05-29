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
        SetPauseMenu(false);
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

    protected override void Start()
    {
        base.Start();

        if (!UnityAdController.showAds)
        {
            SetPauseMenu(false);
        }
    }

    #region Share Score via Twitter "
    /// <summary>
    /// Web address used in order to create a tweet
    /// </summary>
    private const string tweetTextAddress = "http://twitter.com/intent/tweet?text=";

    /// <summary>
    /// Where we want players to visit
    /// </summary>
    private string appStoreLink = "http://johnpdoran.com/";

    [Tooltip("Reference to the player object")]
    public PlayerBehaviour player;

    /// <summary>
    /// Will open twitter with a prebuilt tweet. When When called on IOS or Android
    /// will open the twitter app if installed
    /// </summary>
    public void TweetScore()
    {
        string tweet = "I got " + string.Format("{0:0}", player.Score)
                        + "points at Endless Roller. Can you do better?";

        string message = tweet + "\n" + appStoreLink;

        string url = UnityEngine.Networking.UnityWebRequest.EscapeURL(message);

        Application.OpenURL(tweetTextAddress + url);
    }
    #endregion
}

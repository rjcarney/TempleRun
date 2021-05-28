using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdController : MonoBehaviour, IUnityAdsListener
{
    public static ObstacleBehaviour obstacle;

    /// <summary>
    /// if we should show ads or not
    /// </summary>
    public static bool showAds = true;

    /// <summary>
    /// Replace with your actual game id
    /// </summary>
    private string gameId = "4146173";

    /// <summary>
    /// If the game is in test mode or not
    /// </summary>
    private bool testMode = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!Advertisement.isInitialized)
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
        }
    }

    public static void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }


    #region IUnityAdsListener Methods
    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        PauseMenuBehaviour.paused = true;
        Time.timeScale = 0f;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(obstacle != null && showResult == ShowResult.Finished)
        {
            obstacle.Continue();
        }

        PauseMenuBehaviour.paused = false;
        Time.timeScale = 1f;
    }
    #endregion
}


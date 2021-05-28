using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    /// <summary>
    /// Will load a new scene upon being called
    /// </summary>
    /// <param name="levelName">The name of the level we want to load</param>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);

        if(UnityAdController.showAds)
        {
            UnityAdController.ShowAd();
        }
    }

    public void DisableAds()
    {
        UnityAdController.showAds = false;
        PlayerPrefs.SetInt("Show Ads", 0);
    }

    protected virtual void Start()
    {
        UnityAdController.showAds = (PlayerPrefs.GetInt("Show Ads", 1) == 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObstacleBehaviour : MonoBehaviour
{
    [Tooltip("How long to wait before restarting the game")]
    public float waitTime = 2.0f;

    public GameObject explosion;

    private GameObject player;

    /// <summary>
    /// If object is tapped we spawn an explosion and destroy this object
    /// </summary>
    private void PlayerTouch()
    {
        if(explosion != null)
        {
            var particles = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(particles, 1.0f);
        }

        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            player = collision.gameObject;
            player.SetActive(false);
            
            Invoke("ResetGame", waitTime);
        }
    }

    /// <summary>
    /// Will reset the currently loaded level
    /// </summary>
    private void ResetGame()
    {
        var go = GetGameOverMenu();
        go.SetActive(true);

        var buttons = go.transform.GetComponentsInChildren<Button>();
        Button continueButton = null;

        foreach(var button in buttons)
        {
            if(button.gameObject.name == "Continue Button")
            {
                continueButton = button;
                break;
            }
        }

        if (continueButton)
        {
            if (UnityAdController.showAds)
            {
                continueButton.onClick.AddListener(UnityAdController.ShowAd);
                UnityAdController.obstacle = this;
            }
            else
            {
                continueButton.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Retrieves the Game Over Menu game object
    /// </summary>
    /// <returns>The Game Over Menu game object</returns>
    GameObject GetGameOverMenu()
    {
        var canvas = GameObject.Find("Canvas").transform;
        return canvas.Find("Game Over").gameObject;
    }

    /// <summary>
    /// Handles resetting the game if needed
    /// </summary>
    public void Continue()
    {
        var go = GetGameOverMenu();
        go.SetActive(false);
        player.SetActive(true);

        PlayerTouch();
    }
}

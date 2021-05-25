using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleBehaviour : MonoBehaviour
{
    [Tooltip("How long to wait before restarting the game")]
    public float waitTime = 2.0f;

    public GameObject explosion;

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
            Destroy(collision.gameObject);
            Invoke("ResetGame", waitTime);
        }
    }

    /// <summary>
    /// Will reset the currently loaded level
    /// </summary>
    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

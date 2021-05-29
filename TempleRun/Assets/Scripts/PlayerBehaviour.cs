using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for moving the player automatically
/// and recieving input.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// A reference to the object's Rigidbody component
    /// </summary>
    private Rigidbody rb;

    [Tooltip("How fast the ball moves left/right")]
    public float dodgeSpeed = 5;

    [Tooltip("How fast the ball moves forward automatically")]
    [Range(0, 10)]
    public float runSpeed = 5;

    public enum MobileHorizontalMovement
    {
        Accelerometer,
        ScreenTouch
    }

    public MobileHorizontalMovement horizontalMovement = MobileHorizontalMovement.Accelerometer;

    [Header("Swipe Properties")]
    [Tooltip("How far the player will move upon swiping")]
    public float swipeMove = 2.0f;

    [Tooltip("How far the player must swipe before we execute the action (in inches)")]
    public float minSwipeDistance = 0.25f;

    /// <summary>
    /// used to hold the value on minSwipeDistance converted to pixels
    /// </summary>
    private float minSwipeDistancePixels;

    /// <summary>
    /// Stores the starting position of mobile touch event
    /// </summary>
    private Vector2 touchStart;

    [Header("Scaling Properties")]
    [Tooltip("The minimum size (in Unity units that the player should be")]
    public float minScale = 0.5f;

    [Tooltip("The maximum size (in Unity units that the player should be")]
    public float maxScale = 3.0f;

    /// <summary>
    /// The current scale of the player
    /// </summary>
    private float currentScale = 1;

    [Header("Object References")]
    public Text scoreText;

    private float score = 0;

    public float Score
    {
        get { return score; }
        set
        {
            score = value;
            if(scoreText == null)
            {
                Debug.LogError("Score text is not set.");
                return;
            }
            scoreText.text = string.Format("{0:0}", score);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        minSwipeDistancePixels = minSwipeDistance * Screen.dpi;
        Score = 0;
    }

    // FixedUpdate is called at a fixed framerate
    void FixedUpdate()
    {
        if (PauseMenuBehaviour.paused)
        {
            return;
        }

        // Check if we are moving to the side
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        if(Input.GetMouseButton(0)) {
            horizontalSpeed = CalculateMovement(Input.mousePosition);
        }
        #elif UNITY_IOS || UNITY_ANDROID
        if(horizontalMovement == MobileHorizontalMovement.Accelerometer)
        {
            horizontalSpeed = Input.acceleration.x * dodgeSpeed;
        }

        if(horizontalMovement == MobileHorizontalMovement.ScreenTouch)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                horizontalSpeed = CalculateMovement(touch.position);
            }
        }
        #endif

        rb.AddForce(horizontalSpeed, 0, runSpeed);
    }

    private void Update()
    {
        if (PauseMenuBehaviour.paused)
        {
            return;
        }
        Score += Time.deltaTime;

        #if UNITY_IOS || UNITY_ANDROID
            if(Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                SwipeTeleport(touch);

                TouchObjects(touch);

                ScalePlayer();
            }
        #endif
    }

    /// <summary>
    /// Will figure out where to move the player horizontally
    /// </summary>
    /// <param name="pixelPos">The position the player has touched/clicked on</param>
    /// <returns>The direction to move in the x axis</returns>
    private float CalculateMovement(Vector3 pixelPos)
    {
        var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);
        float xMove = 0;

        if (worldPos.x < 0.5f)
        {
            xMove = -1;
        }
        else
        {
            xMove = 1;
        }
        return xMove * dodgeSpeed;
    }

    /// <summary>
    /// Will teleport the player if swiped left or right
    /// </summary>
    /// <param name="touch">Current touch event</param>
    private void SwipeTeleport(Touch touch)
    {
        if(touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }
        else if(touch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnd = touch.position;
            float x = touchEnd.x - touchStart.x;

            if(Mathf.Abs(x) < minSwipeDistance)
            {
                return;
            }

            Vector3 moveDirection;
            if (x < 0)
                moveDirection = Vector3.left;
            else
                moveDirection = Vector3.right;

            RaycastHit hit;
            if(!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (moveDirection * swipeMove));
            }
        }
    }

    /// <summary>
    /// Will teleport the player if swiped left or right
    /// </summary>
    private void ScalePlayer()
    {
        if(Input.touchCount != 2)
        {
            return;
        }
        else
        {
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0Prev - touch1Prev).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float newScale = currentScale - (deltaMagnitudeDiff * Time.deltaTime);

            newScale = Mathf.Clamp(newScale, minScale, maxScale);

            transform.localScale = Vector3.one * newScale;

            currentScale = newScale;
        }
    }


    /// <summary>
    /// Will determine if we are touching a game object and if so
    /// call events for it
    /// </summary>
    /// <param name="touch">Current touch event</param>
    private static void TouchObjects(Touch touch)
    {
        Ray touchRay = Camera.main.ScreenPointToRay(touch.position);

        RaycastHit hit;

        int layerMask = ~0;

        if(Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }

}

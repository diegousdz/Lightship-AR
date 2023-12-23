using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 startTouchPosition, endTouchPosition;
    private Vector2 startMousePosition, endMousePosition;
    private bool isSwiping = false;

    public float minSwipeDistance = 50f; // Minimum distance for a swipe to be registered
    public float maxSwipeTime = 1f; // Maximum time in seconds for a swipe gesture

    private float swipeStartTime;

    void Update()
    {
        // Touch input (for mobile devices)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isSwiping = true;
                    startTouchPosition = touch.position;
                    swipeStartTime = Time.time;
                    break;

                case TouchPhase.Ended:
                    if (isSwiping)
                    {
                        endTouchPosition = touch.position;
                        DetectSwipe(startTouchPosition, endTouchPosition);
                    }
                    break;
            }
        }

        // Mouse input (for Unity Editor or desktop platforms)
        if (Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
            startMousePosition = Input.mousePosition;
            swipeStartTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            endMousePosition = Input.mousePosition;
            DetectSwipe(startMousePosition, endMousePosition);
        }
    }

    private void DetectSwipe(Vector2 start, Vector2 end)
    {
        float swipeDistance = Vector2.Distance(start, end);
        float swipeTime = Time.time - swipeStartTime;

        if (swipeTime < maxSwipeTime && swipeDistance > minSwipeDistance)
        {
            Vector2 direction = end - start;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Horizontal swipe
                if (direction.x > 0)
                    Debug.Log("Swipe Right");
                else
                    Debug.Log("Swipe Left");
            }
            else
            {
                // Vertical swipe
                if (direction.y > 0)
                    Debug.Log("Swipe Up");
                else
                    Debug.Log("Swipe Down");
            }
        }

        // Reset
        isSwiping = false;
    }
}

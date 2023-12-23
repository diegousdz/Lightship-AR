using UnityEngine;
using UnityEngine.Events;

public class TapDetector : MonoBehaviour
{
    public int tapCountThreshold = 2; // Number of taps to trigger the event
    public float tapInterval = 0.5f; // Maximum interval between taps in seconds

    public UnityEvent onTapThresholdReached; // Event to invoke when tap threshold is reached

    private int tapCount = 0;
    private float lastTapTime = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (Time.time - lastTapTime <= tapInterval)
            {
                tapCount++;
            }
            else
            {
                tapCount = 1;
            }

            lastTapTime = Time.time;

            if (tapCount >= tapCountThreshold)
            {
                onTapThresholdReached.Invoke();
                tapCount = 0; // Reset tap count after reaching threshold
            }
        }
    }

    public void say(){
        print("say hello");
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleModifier : MonoBehaviour
{
    public float timeScaleIncrease = 2.0f; // The amount by which to increase the time scale
    public float increaseDuration = 25.0f; // The duration for which the time scale is increased

    private Coroutine timeScaleCoroutine = null;
    private float timer = 0.0f;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StartIncreasingTimeScale()
    {
        // If a coroutine is already running, reset the timer
        if (timeScaleCoroutine != null)
        {
            timer = increaseDuration;
        }
        else // Start a new coroutine
        {
            timer = increaseDuration;
            timeScaleCoroutine = StartCoroutine(IncreaseTimeScale());
        }
    }

    IEnumerator IncreaseTimeScale()
    {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = timeScaleIncrease;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime; // Changed Time.deltaTime to Time.unscaledDeltaTime
            yield return null;
        }

        Time.timeScale = 1f; // Reset to originalTimeScale instead of 1f
        timeScaleCoroutine = null;
    }

}

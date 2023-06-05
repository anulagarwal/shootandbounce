using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleModifier : MonoBehaviour
{
    public float timeScaleIncrease = 2.0f; // The amount by which to increase the time scale
    public float increaseDuration = 2.0f; // The duration for which the time scale is increased
    public Button speedUpButton; // Assign the button in the Inspector

    private Coroutine timeScaleCoroutine = null;
    private float timer = 0.0f;

    private void Start()
    {
        speedUpButton.onClick.AddListener(StartIncreasingTimeScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speedUpButton.onClick.Invoke();
            //simulate button pressed visual effect
            speedUpButton.GetComponent<Image>().color = Color.gray;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //reset button color after spacebar released
            speedUpButton.GetComponent<Image>().color = Color.white;
        }
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
            timer -= Time.deltaTime;
            yield return null;
        }

        Time.timeScale = originalTimeScale;
        timeScaleCoroutine = null;
    }
}

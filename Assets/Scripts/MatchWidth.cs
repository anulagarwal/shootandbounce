using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MatchWidth : MonoBehaviour
{
    public float targetAspect = 1080f / 1920f; // Set this to the aspect ratio that you designed your game for.
    public float targetOrthographicSize = 5f; // Set this to your desired Orthographic Size at your target aspect ratio.
    public float maxNarrowOrthographicSize = 7f; // The maximum Orthographic Size when the screen is "narrow".


    public float minAspect = 480f / 800f; // Minimum aspect ratio
    public float maxAspect = 1440f / 2960f; // Maximum aspect ratio
    public float minOrthographicSize = 14f; // The minimum Orthographic Size.
    public float maxOrthographicSize = 17f; // The maximum Orthographic Size.

    public Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    void Update()
    {
        AdjustOrthographicSize();
    }

    void AdjustOrthographicSize()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;

        if (currentAspect < targetAspect)
        {
            // If the screen is "narrow" (height > width), use the Orthographic Size that is proportionally between target and max narrow size
           /* float aspectRatioDifference = targetAspect - currentAspect;
            float sizeDifference = maxNarrowOrthographicSize - targetOrthographicSize;
            cam.orthographicSize = targetOrthographicSize + (aspectRatioDifference * sizeDifference);
           */
            float currentAspecta = (float)Screen.width / (float)Screen.height;

            // Clamping the current aspect ratio between min and max aspects
            float clampedAspect = Mathf.Clamp(currentAspecta, minAspect, maxAspect);
            float i = Mathf.InverseLerp(minAspect, maxAspect, clampedAspect);

            print(currentAspecta);
            // Interpolate the orthographic size between the minimum and maximum based on the aspect ratio proximity
            cam.orthographicSize = Mathf.Lerp(minOrthographicSize, maxOrthographicSize, i);

        }
        else
        {
            // Calculate Orthographic Size adjustment for wide screens
            cam.orthographicSize = targetOrthographicSize * Mathf.Sqrt(targetAspect / currentAspect);
        }
    }
}

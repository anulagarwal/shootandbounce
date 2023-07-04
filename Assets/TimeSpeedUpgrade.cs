using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TimeSpeedUpgrade : MonoBehaviour
{
    public GameObject targetGameObject;
    public Image timerImage;

    public float enableDuration = 8f;  // Time in seconds for which the object will be enabled
    public float cycleTime = 90f; // Total time in seconds for one cycle (enable + disable)

    private Vector3 originalScale;
    public float punchScaleMagnitude = 1.2f;  // How much the scale will increase
    public float punchScaleDuration = 0.5f;  // How long the punch scale effect will last
    private Tween scaleTween;

    private Coroutine enableDisableRoutine = null; // this variable will hold the reference to the coroutine
    private Coroutine ongoingRoutine = null;
    private Coroutine timerRoutine = null;

    private void Start()
    {
        if (targetGameObject == null)
        {
            Debug.LogError("Target GameObject is not assigned");
            return;
        }

        if (timerImage == null)
        {
            Debug.LogError("Timer Image is not assigned");
            return;
        }

        originalScale = targetGameObject.transform.localScale;

        enableDisableRoutine = StartCoroutine(EnableDisableRoutine());
        ongoingRoutine = StartCoroutine(EnableDisableRoutine());
        // Punch scale loop
        scaleTween = targetGameObject.transform.DOScale(originalScale * punchScaleMagnitude, punchScaleDuration)
        .SetEase(Ease.OutBounce).SetLoops(-1, LoopType.Yoyo);
    }

    private IEnumerator EnableDisableRoutine()
    {
        while (true)
        {
            targetGameObject.SetActive(true);
            timerRoutine = StartCoroutine(UpdateTimerImage());
            scaleTween = targetGameObject.transform.DOScale(originalScale * punchScaleMagnitude, punchScaleDuration)
       .SetEase(Ease.OutBounce).SetLoops(-1, LoopType.Yoyo);
            yield return new WaitForSeconds(enableDuration);
            // Stop the scaling loop when the object is disabled

            targetGameObject.transform.localScale = originalScale;
            targetGameObject.SetActive(false);
            yield return new WaitForSeconds(cycleTime - enableDuration);
        }
    }
    float currentTime = 0f;
    private IEnumerator UpdateTimerImage()
    {

        currentTime = 0f;
        while (currentTime <= enableDuration)
        {
            currentTime += Time.deltaTime;
            timerImage.fillAmount = 1f - (currentTime / enableDuration);

            if (currentTime >= enableDuration)
            {
                targetGameObject.SetActive(false);              
            }
            yield return null;
        }
    }

    public void ResetAndDisableGameObject()
    {
        currentTime = enableDuration - 0.1f;    
        targetGameObject.SetActive(false);

        /*
        if (ongoingRoutine != null)
        {
            StopCoroutine(ongoingRoutine);
            ongoingRoutine = null;
        }

        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }

        // Immediately disable the object and reset the image fill amount
        targetGameObject.SetActive(false);
        timerImage.fillAmount = 1f;

        // Wait for the remaining time in the cycle before enabling the GameObject again
        ongoingRoutine = StartCoroutine(ResetAndEnableRoutine());
        */

    }
    private IEnumerator ResetAndEnableRoutine()
    {
        yield return new WaitForSeconds(cycleTime - enableDuration);
        ongoingRoutine = StartCoroutine(EnableDisableRoutine());
    }
}

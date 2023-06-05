using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Singleton instance
    public static TutorialManager Instance { get; private set; }

    // You can add more steps as required in your tutorial
    public enum TutorialStep { Step1, Step2, Step3, Complete }
    public int currentStep;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensures that the object survives when loading a new scene
        }
        else
        {
            Destroy(gameObject); // If a Singleton already exists, destroy this one
        }
    }

    [Header("Component")]
    [SerializeField] Animator anim;
    [SerializeField] int maxSteps;
    [SerializeField] public bool isEnabled;
    [SerializeField] public bool isActive;

    [SerializeField] GameObject tutorialObject;
    [SerializeField] List<GameObject> tutorialNotNeeded;

    // Start is called before the first frame update
    void Start()
    {
        if (isEnabled)
        {
            
            EnableTutorial();
          
        }
        else
        {
            DisableTutorial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableTutorial()
    {
        currentStep = 0;
        foreach(GameObject g in tutorialNotNeeded)
        {
            g.SetActive(false);
        }
        tutorialObject.SetActive(true);
        isActive = true;
        anim.enabled = true;
        NextStep();
    }

    public void DisableTutorial()
    {
        foreach (GameObject g in tutorialNotNeeded)
        {
            g.SetActive(true);
        }
        GunSelectionGridManager.Instance.ActiveButton(true);
        PlayerPrefs.SetInt("tutorial", 0);
        tutorialObject.SetActive(false);
        isActive = false;
       
    }

    public void NextStep()
    {
        currentStep++;
        switch (currentStep)
        {
            case 1:
                GunSelectionGridManager.Instance.ActiveButton(true);
                break;

            case 2:
                GunSelectionGridManager.Instance.ActiveButton(false);
                break;

            case 3:
                GunSelectionGridManager.Instance.ActiveButton(true);
                break;

            case 4:
                GunSelectionGridManager.Instance.ActiveButton(false);
                break;
        }
        if (currentStep > maxSteps)
        {
            DisableTutorial();
            return;
        }

        anim.Play("Step" + currentStep);

    }
}

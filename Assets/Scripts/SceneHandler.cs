using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevels;
    [SerializeField] private bool changeManual;


    // Start is called before the first frame update
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 1);

        if (!changeManual)
        {
            if (currentLevel > maxLevels)
            {
                int newId = currentLevel % maxLevels;
                if (newId == 0)
                {
                    newId = maxLevels;
                }
                SceneManager.LoadScene("Level " + (newId));
            }
            else
            {
                SceneManager.LoadScene("Level " + currentLevel);
            }
        }
    }

    public void ChangeLevel()
    {
        
            AdManager.Instance.ShowAdChangeLevel();
         
    }

    public void ChangeTheLevel()
    {
        if (currentLevel > maxLevels)
        {
            int newId = currentLevel % maxLevels;
            if (newId == 0)
            {
                newId = maxLevels;
            }
            SceneManager.LoadScene("Level " + (newId));
        }
        else
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }
    }
}

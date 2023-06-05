using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CrazyGames;
using UnityEngine.UI;
using TMPro;
public class SceneHandler : MonoBehaviour
{
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevels;
    [SerializeField] private bool changeManual;
    [SerializeField] Button b;
    [SerializeField] TextMeshProUGUI t;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Slider soundSlider;




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
        UpdateSoundSlider();

        levelText.text = "LEVEL " + (currentLevel);
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

    public void UpdateSound()
    {
        PlayerPrefs.SetFloat("soundvalue", soundSlider.value);
    }
    public void UpdateSoundSlider()
    {
        soundSlider.value = PlayerPrefs.GetFloat("soundvalue", 1);
    }


    public void WatchAdForCoin()
    {
        CrazyAds.Instance.beginAdBreakRewarded(GiveCoins);
    }

    public void GiveCoins()
    {
        b.interactable = false;
        t.alpha = 0.5f;
        int curCoins = PlayerPrefs.GetInt("coins");
        curCoins += 500;
        PlayerPrefs.SetInt("coins",curCoins);
        //Disable Button
        //Give Coins
    }
}

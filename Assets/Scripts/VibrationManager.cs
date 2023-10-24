using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Lofelt.NiceVibrations;
public class VibrationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMedium()
    {
        if (PlayerPrefs.GetInt("vibrate", 1) == 1)
        {
           // HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
    }
}

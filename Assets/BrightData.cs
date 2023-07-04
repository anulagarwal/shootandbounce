using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightData : MonoBehaviour
{

    

    // Start is called before the first frame update
    void Start()
    {
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");


        /*Important when testing the code: currentActivity will be available only on a device/emulator and not inside Unity*/
        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");


        //Use the full package name, If you have a dedicated package name update it accordingly
        AndroidJavaObject brightApi = new AndroidJavaObject("com.android.eapx.main");


        brightApi.CallStatic("start", currentActivity);

        brightApi.CallStatic("show_dialog", currentActivity, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

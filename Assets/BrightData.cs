using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightData : MonoBehaviour
{

    AndroidJavaObject brightApi;
    AndroidJavaObject currentActivity;
    AndroidJavaClass unityPlayerClass;
    AndroidJavaObject settings;
    ChoiceListener choiceListener;
    // Start is called before the first frame update
    void Start()
    {
        /*unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        /*Important when testing the code: currentActivity will be available only on a device/emulator and not inside Unity
        currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

         brightApi = new AndroidJavaObject("com.android.eapx.BrightApi");

        settings = new AndroidJavaObject("com.android.eapx.Settings");
        choiceListener = new ChoiceListener();
        OptIn();*/
    }

    public void OptIn()
    {
        settings.Call("setSkipConsent", true);
        //optional
        settings.Call("setBenefit", "Skip forced ads by opt-in!");
        settings.Call("setAgreeBtn", "YES, NO ADS!");
        settings.Call("setDisagreeBtn", "SHOW ME ADS");

        settings.Call("setOnStatusChange", choiceListener);
        // init the SDK
        brightApi.CallStatic("init", currentActivity, settings);

        // show the consent dialog - if setSkipConsent was set to true
        brightApi.CallStatic("showConsent", currentActivity);
    }
    public void OptOut()
    {
        brightApi.CallStatic("optOut", currentActivity); //this is for optout
    }

  
}

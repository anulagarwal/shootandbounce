using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class ChoiceListener : AndroidJavaProxy
{
    public ChoiceListener() : base("com.android.eapx.Settings$OnStatusChange") { }
    void onChange(int choice)
    {
        //Here you'll get the callback with the user's choice
        // 1 - user agreed
        // 4 - user declined
        
    }
}
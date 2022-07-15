using UnityEngine;
using Facebook.Unity;
using System;

public class MMFacebookManager
{
    static MMFacebookManager _instance;

    public static MMFacebookManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new MMFacebookManager();

            return _instance;
        }
    }

    Action _onInited;

    bool _isInited;

    public bool IsInited(Action callback)
    {
        if (FB.IsInitialized)
            return true;

        _onInited += callback;

        return false;
    }

    public MMFacebookManager()
    {
        InitFB();
    }

    void InitFB()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(OnInitComplete, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void OnInitComplete()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...

            if (!_isInited)
            {
                _isInited = true;

                if (_onInited != null)
                {
                    _onInited();
                    _onInited = null;
                }
            }
        }
        else
        {
            Logger.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        /*this.Status = "Success - Check log for details";
        this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
        LogView.AddLog("Is game shown: " + isGameShown);*/
    }
}

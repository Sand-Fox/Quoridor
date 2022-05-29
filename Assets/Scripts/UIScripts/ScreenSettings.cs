using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour
{
    private Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = Screen.fullScreen;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Resolution maxResolution = Screen.resolutions[Screen.resolutions.Length - 1];
        Resolution bigResolution = new Resolution();
        Resolution littleResolution = new Resolution();

        if (maxResolution.width == 1920 && maxResolution.height == 1080)
        {
            bigResolution.width = 1920;
            bigResolution.height = 1080;
            littleResolution.width = 1600;
            littleResolution.height = 900;
        }
        else
        {
            bigResolution.width = 2560;
            bigResolution.height = 1440;
            littleResolution.width = 1920;
            littleResolution.height = 1080;
        }

        if (isFullScreen) Screen.SetResolution(bigResolution.width, bigResolution.height, true);
        else Screen.SetResolution(littleResolution.width, littleResolution.height, false);
    }
}

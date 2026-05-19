using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WindowControls : Controls
{
    [SerializeField]
    private string windowOptionsMain, fullScreen, windowed;

    public string WindowOptionsMain { get { return windowOptionsMain; } set { windowOptionsMain = value; } }
    public string FullScreen { get { return fullScreen; } set { fullScreen = value; } }
    public string Windowed { get { return windowed; } set { windowed = value; } }

    [SerializeField]
    private string windowSize;

    public string WindowSize { get { return windowSize; } set { windowSize = value; } }

    [SerializeField]
    private int screenWidth, screenHeight;

    private void Start()
    {
        screenWidth = screenWidth == 0 ? 1208 : screenWidth;
        screenHeight = screenHeight == 0 ? 720 : screenHeight;

        switch(windowSize)
        {
            case "window":
                windowSize = "window";
                break;
            case "full":
                windowSize = "full";
                break;
            default: 
                windowSize = "full";
                break;
        }

        LoadWindowSize(windowSize);
    }

    public void LoadWindowSize(string size)
    {
        if(size == "window")
        {
            Screen.SetResolution(screenWidth, screenHeight, FullScreenMode.Windowed);
        }
        else
        {
            Screen.fullScreen = true;
        }
    }

    public void SetFullScreenWindow(ClickEvent evt)
    {
        windowSize = "full";
        Screen.fullScreen = true;
    }

    public void SetWindowedScreenWindow(ClickEvent evt)
    {
        windowSize = "window";
        Screen.SetResolution(screenWidth, screenHeight, FullScreenMode.Windowed);
    }

    public override void ShowcaseOptions(VisualElement window)
    {
        base.ShowcaseOptions(window);
    }
}
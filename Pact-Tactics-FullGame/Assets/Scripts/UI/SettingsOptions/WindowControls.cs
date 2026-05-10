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
    private int screenWidth, screenHeight;

    private void Start()
    {
        screenWidth = screenWidth == 0 ? 1208 : screenWidth;
        screenHeight = screenHeight == 0 ? 720 : screenHeight;
    }

    public void SetFullScreenWindow(ClickEvent evt)
    {
        Debug.Log("Set full screen");
        Screen.fullScreen = true;
    }

    public void SetWindowedScreenWindow(ClickEvent evt)
    {
        Debug.Log("Set window screen");
        Screen.SetResolution(screenWidth, screenHeight, FullScreenMode.Windowed);
    }

    public override void ShowcaseOptions(VisualElement window)
    {
        base.ShowcaseOptions(window);
    }
}
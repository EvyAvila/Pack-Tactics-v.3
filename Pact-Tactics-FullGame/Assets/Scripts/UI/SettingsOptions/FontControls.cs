using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FontControls : Controls
{
    [SerializeField]
    private string fontMain, audioMain, windowMain, fontOptionsMain;

    //Take this out of here. Put in another script
    public string FontMain { get { return fontMain; } set { fontMain = value; } }
    public string AudioMain { get { return audioMain; } set { audioMain = value; } }
    public string WindowMain { get { return windowMain; } set { windowMain = value; } }

    public string FontOptionsMain { get { return fontOptionsMain; } set { fontOptionsMain = value; } }
    
    public override void ShowcaseOptions(VisualElement window)
    {
        base.ShowcaseOptions(window);
    }
}

public abstract class Controls : MonoBehaviour 
{
    public bool isDisplayed { get; set; }

    public virtual void ShowcaseOptions(VisualElement window)
    {
        if (!isDisplayed)
        {
            window.style.display = DisplayStyle.Flex;
        }
        else
        {
            window.style.display = DisplayStyle.None;
        }

        isDisplayed = !isDisplayed;
    }
}

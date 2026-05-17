using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Yarn.Unity;

public class FontControls : Controls
{
    [SerializeField]
    private string fontOptionsMain;

    [SerializeField]
    private string smallTxt, mediumTxt, largeTxt;

    [SerializeField]
    private FontSizeEvent fontSizeEvent;

    [SerializeField]
    private float smallTextSize, mediumTextSize, largeTextSize;

    public string FontOptionsMain { get { return fontOptionsMain; } set { fontOptionsMain = value; } }

    public string SmallTxt { get { return smallTxt; } set { smallTxt = value; } }
    public string MediumTxt { get { return mediumTxt; } set { mediumTxt = value; } }
    public string LargeTxt { get { return largeTxt; } set { largeTxt = value; } }

    public DialogueRunner dialogueRunner;

    private void Awake()
    {
        StopTalking(); //Should this be in Start instead?
    }

    private void Start()
    {
        smallTextSize = smallTextSize == 0 ? 40 : smallTextSize;
        mediumTextSize = mediumTextSize == 0 ? 80 : mediumTextSize;
        largeTextSize = largeTextSize == 0 ? 120 : largeTextSize;
        
    }

    public override void ShowcaseOptions(VisualElement window)
    {

        if (!isDisplayed)
        {
            window.style.display = DisplayStyle.Flex;
            Talk();
        }
        else
        {
            window.style.display = DisplayStyle.None;
            StopTalking();
        }

        isDisplayed = !isDisplayed;
    }

    public void Talk()
    {
        dialogueRunner.StartDialogue("Start");
    }

    public void StopTalking()
    {
        dialogueRunner.Stop();
    }

    public void SetSmallTextSize(ClickEvent evt)
    {
        fontSizeEvent.UpdateSizeValue(smallTextSize);
    }

    public void SetMediumTextSize(ClickEvent evt)
    {
        fontSizeEvent.UpdateSizeValue(mediumTextSize);
    }

    public void SetLargeTextSize(ClickEvent evt)
    {
        fontSizeEvent.UpdateSizeValue(largeTextSize);
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

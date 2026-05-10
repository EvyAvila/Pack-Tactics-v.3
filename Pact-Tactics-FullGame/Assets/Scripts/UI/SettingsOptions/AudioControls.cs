using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioControls : Controls
{
    [SerializeField]
    private string audioOptionMain;

    public string AudioOptionMain { get { return audioOptionMain; } set { audioOptionMain = value; } }

    public override void ShowcaseOptions(VisualElement window)
    {
        base.ShowcaseOptions(window);
    }
}

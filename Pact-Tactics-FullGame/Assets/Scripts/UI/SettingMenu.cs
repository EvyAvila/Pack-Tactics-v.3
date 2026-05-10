using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor.Search;


public class SettingMenu : AbstractMenuBase
{
    private FontControls fontControls;
    private AudioControls audioControls;
    private WindowControls windowControls;

    private Button exitBtn;

    private Button FontMainBtn, AudioMainBtn, WindowMainBtn;
    
    private Button windowBtn, fullscreenBtn;

    private VisualElement windowOptions, audioOptions, fontOptions;


    public override void SetProperties()
    {
        SetUpControlScripts();

        exitBtn = root.Q<Button>("ExitBtn");
        exitBtn.RegisterCallback<ClickEvent>(ExitSettingsMenu);

        SetUpFontsControls();

        SetUpAudioControls();

        SetUpWindowControls();
    }

    public override void UnSetProperties()
    {
        exitBtn.UnregisterCallback<ClickEvent>(ExitSettingsMenu);

        //Fonts
        FontMainBtn.UnregisterCallback<ClickEvent>(DisplayFontSection);
        AudioMainBtn.UnregisterCallback<ClickEvent>(DisplayAudioSection);
        WindowMainBtn.UnregisterCallback<ClickEvent>(DisplayWindowSection);

        //Audio

        //Window
        windowBtn.UnregisterCallback<ClickEvent>(windowControls.SetWindowedScreenWindow);
        fullscreenBtn.UnregisterCallback<ClickEvent>(windowControls.SetFullScreenWindow);
    }

    private void ExitSettingsMenu(ClickEvent evt)
    {
        CustomizedEventActions.OnRequestUnloadScene?.Invoke(SceneType.Settings);
    }

    private void DisplayFontSection(ClickEvent evt)
    {
        if (windowOptions.visible)
        {
            SetVisualElementBool(windowOptions, windowControls);
        }
        if (audioOptions.visible)
        {
            SetVisualElementBool(audioOptions, audioControls);
        }

        fontControls.ShowcaseOptions(fontOptions);
    }

    private void DisplayAudioSection(ClickEvent evt)
    {
        if(windowOptions.visible)
        {
            SetVisualElementBool(windowOptions, windowControls);
        }
        if (fontOptions.visible)
        {
            SetVisualElementBool(fontOptions, fontControls);
        }

        audioControls.ShowcaseOptions(audioOptions);
    }

    private void DisplayWindowSection(ClickEvent evt)
    {
        if(audioOptions.visible)
        {
            SetVisualElementBool(audioOptions, audioControls);

        }
        if(fontOptions.visible)
        {
            SetVisualElementBool(fontOptions, fontControls);
        }

        windowControls.ShowcaseOptions(windowOptions);
    }

    private void SetVisualElementBool(VisualElement section, Controls control)
    {
        section.style.display = DisplayStyle.None;
        control.isDisplayed = false;
    }

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    //Scripts
    private void SetUpControlScripts()
    {
        if (fontControls == null) { fontControls = GetComponent<FontControls>(); }
        if (audioControls == null) { audioControls = GetComponent<AudioControls>(); }
        if (windowControls == null) { windowControls = GetComponent<WindowControls>(); }
    }

    //Fonts
    private void SetUpFontsControls()
    {
        //Fonts
        fontOptions = root.Q<VisualElement>(fontControls.FontOptionsMain);

        FontMainBtn = root.Q<Button>(fontControls.FontMain);
        FontMainBtn.RegisterCallback<ClickEvent>(DisplayFontSection);

        AudioMainBtn = root.Q<Button>(fontControls.AudioMain);
        AudioMainBtn.RegisterCallback<ClickEvent>(DisplayAudioSection);

        WindowMainBtn = root.Q<Button>(fontControls.WindowMain);
        WindowMainBtn.RegisterCallback<ClickEvent>(DisplayWindowSection);
    }

    //Windows
    private void SetUpWindowControls()
    {
        //Window
        windowOptions = root.Q<VisualElement>(windowControls.WindowOptionsMain);

        windowBtn = root.Q<Button>(windowControls.Windowed);
        windowBtn.RegisterCallback<ClickEvent>(windowControls.SetWindowedScreenWindow);

        fullscreenBtn = root.Q<Button>(windowControls.FullScreen);
        fullscreenBtn.RegisterCallback<ClickEvent>(windowControls.SetFullScreenWindow);
    }

    //Audio
    private void SetUpAudioControls()
    {
        audioOptions = root.Q<VisualElement>(audioControls.AudioOptionMain);
    }
}
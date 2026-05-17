using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;


public class SettingMenu : AbstractMenuBase
{
    [SerializeField]
    private SaveManager saveSettingsInfo;

    private FontControls fontControls;
    private AudioControls audioControls;
    private WindowControls windowControls;

    private Button exitBtn;

    private Button FontMainBtn, AudioMainBtn, WindowMainBtn;

    private Button smallTxtBtn, mediumTxtBtn, largeTxtBtn;
    private Button windowBtn, fullscreenBtn;

    private VisualElement windowOptions, audioOptions, fontOptions;
    private VisualElement audioMusic, audioSFX, audioVoiceAct;

    [SerializeField]
    private string fontMain, audioMain, windowMain;
    public string FontMain { get { return fontMain; } set { fontMain = value; } }
    public string AudioMain { get { return audioMain; } set { audioMain = value; } }
    public string WindowMain { get { return windowMain; } set { windowMain = value; } }


    public override void SetProperties()
    {
        SetUpControlScripts();

        exitBtn = root.Q<Button>("ExitBtn");
        exitBtn.RegisterCallback<ClickEvent>(ExitSettingsMenu);

        SetUpFontsControls();

        SetUpAudioControls();

        SetUpWindowControls();

        saveSettingsInfo.LoadPreference();
    }

    public override void UnSetProperties()
    {
        exitBtn.UnregisterCallback<ClickEvent>(ExitSettingsMenu);

        //Main Options
        FontMainBtn.UnregisterCallback<ClickEvent>(DisplayFontSection);
        AudioMainBtn.UnregisterCallback<ClickEvent>(DisplayAudioSection);
        WindowMainBtn.UnregisterCallback<ClickEvent>(DisplayWindowSection);

        //Fonts
        smallTxtBtn.UnregisterCallback<ClickEvent>(fontControls.SetSmallTextSize);
        mediumTxtBtn.UnregisterCallback<ClickEvent>(fontControls.SetMediumTextSize);
        largeTxtBtn.UnregisterCallback<ClickEvent>(fontControls.SetLargeTextSize);

        //Audio
        audioControls.UnsetButtonsList();

        //Window
        windowBtn.UnregisterCallback<ClickEvent>(windowControls.SetWindowedScreenWindow);
        fullscreenBtn.UnregisterCallback<ClickEvent>(windowControls.SetFullScreenWindow);
    }

    private void ExitSettingsMenu(ClickEvent evt)
    {
        saveSettingsInfo.SavePreference();
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
        if (windowOptions.visible)
        {
            SetVisualElementBool(windowOptions, windowControls);
        }
        if (fontOptions.visible)
        {
            SetVisualElementBool(fontOptions, fontControls);
            fontControls.StopTalking();
        }

        audioControls.ShowcaseOptions(audioOptions);
    }

    private void DisplayWindowSection(ClickEvent evt)
    {
        if (audioOptions.visible)
        {
            SetVisualElementBool(audioOptions, audioControls);

        }
        if (fontOptions.visible)
        {
            SetVisualElementBool(fontOptions, fontControls);
            fontControls.StopTalking();
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
        //Options
        fontOptions = root.Q<VisualElement>(fontControls.FontOptionsMain);

        FontMainBtn = root.Q<Button>(FontMain);
        FontMainBtn.RegisterCallback<ClickEvent>(DisplayFontSection);

        AudioMainBtn = root.Q<Button>(AudioMain);
        AudioMainBtn.RegisterCallback<ClickEvent>(DisplayAudioSection);

        WindowMainBtn = root.Q<Button>(WindowMain);
        WindowMainBtn.RegisterCallback<ClickEvent>(DisplayWindowSection);
        
        //Fonts
        smallTxtBtn = root.Q<Button>(fontControls.SmallTxt);
        smallTxtBtn.RegisterCallback<ClickEvent>(fontControls.SetSmallTextSize);
        mediumTxtBtn = root.Q<Button>(fontControls.MediumTxt);
        mediumTxtBtn.RegisterCallback<ClickEvent>(fontControls.SetMediumTextSize);
        largeTxtBtn = root.Q<Button>(fontControls.LargeTxt);
        largeTxtBtn.RegisterCallback<ClickEvent>(fontControls.SetLargeTextSize);
        fontControls.StopTalking();
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
        
        audioMusic = root.Q<VisualElement>(audioControls.MusicVolume);
        audioSFX = root.Q<VisualElement>(audioControls.SfxVolume);
        audioVoiceAct = root.Q<VisualElement>(audioControls.VoiceActingVolume);


        audioControls.SetButtonLists(audioMusic, audioSFX, audioVoiceAct);
        
    }
}
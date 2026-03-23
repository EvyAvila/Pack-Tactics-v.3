using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeMenu : BaseMenu
{
    private Button playBtn, settingBtn, quitBtn;

    protected override void Awake()
    {
        scriptName = SceneScript.HomeMenu;
    }

    protected override void SetProperties()
    {
        playBtn = root.Q<Button>("PlayBtn");
        settingBtn = root.Q<Button>("SettingsBtn");
        quitBtn = root.Q<Button>("QuitBtn");

        playBtn.RegisterCallback<ClickEvent>(EnterGame);
        settingBtn.RegisterCallback<ClickEvent>(OpenSettings);
        quitBtn.RegisterCallback<ClickEvent>(QuitGame);
    }

    protected override void UnSetProperties()
    {
        playBtn.UnregisterCallback<ClickEvent>(EnterGame);
        settingBtn.UnregisterCallback<ClickEvent>(OpenSettings);
        quitBtn.UnregisterCallback<ClickEvent>(QuitGame);
    }

    private void EnterGame(ClickEvent evt)
    {
        Debug.Log("play game here");
    }

    private void OpenSettings(ClickEvent evt)
    {
        Debug.Log("Open settings");
    }

    private void QuitGame(ClickEvent evt)
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

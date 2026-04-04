using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeMenu : AbstractMenuBase
{
    private Button playBtn, settingBtn, quitBtn;

    public override void SetProperties()
    {
        playBtn = root.Q<Button>("PlayBtn");
        settingBtn = root.Q<Button>("SettingsBtn");
        quitBtn = root.Q<Button>("QuitBtn");

        playBtn.RegisterCallback<ClickEvent>(EnterGame);
        settingBtn.RegisterCallback<ClickEvent>(OpenSettings);
        quitBtn.RegisterCallback<ClickEvent>(QuitGame);
    }

    public override void UnSetProperties()
    {
        playBtn.UnregisterCallback<ClickEvent>(EnterGame);
        settingBtn.UnregisterCallback<ClickEvent>(OpenSettings);
        quitBtn.UnregisterCallback<ClickEvent>(QuitGame);
    }

    private void EnterGame(ClickEvent evt)
    {
        StateManager.Instance.ChangeState(GameState.Game);
    }

    private void OpenSettings(ClickEvent evt)
    {
        CustomizedEventActions.OnRequestSceneLoad?.Invoke(SceneType.Settings);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingMenu : AbstractMenuBase
{
    private Button exitBtn;

    public override void SetProperties()
    {
        exitBtn = root.Q<Button>("ExitBtn");

        exitBtn.RegisterCallback<ClickEvent>(ExitSettingsMenu);
    }

    public override  void UnSetProperties()
    {
        exitBtn.UnregisterCallback<ClickEvent>(ExitSettingsMenu);
    }

    private void ExitSettingsMenu(ClickEvent evt)
    {
        CustomizedEventActions.OnRequestUnloadScene?.Invoke(SceneType.Settings);
    }
}

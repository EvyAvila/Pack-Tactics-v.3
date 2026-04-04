using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMenu : AbstractMenuBase//MonoBehaviour
{
    private Button saveBtn;

    public override void SetProperties()
    {
        saveBtn = root.Q<Button>("SaveBtn");

        saveBtn.RegisterCallback<ClickEvent>(ExitSaveMenu);
    }

    public override void UnSetProperties()
    {
        saveBtn.UnregisterCallback<ClickEvent>(ExitSaveMenu);
    }

    private void ExitSaveMenu(ClickEvent evt)
    {
        CustomizedEventActions.OnRequestSceneLoad?.Invoke(SceneType.Save);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingMenu : MonoBehaviour
{

    private Button exitBtn;

    public VisualElement root;
    public UIDocument uiDocument;



    private void OnEnable()
    {
        if (uiDocument == null)
        {
            Debug.LogError("PauseMenu: UIDocument is not assigned!");
            return;
        }

        StartCoroutine(SetupUI());
    }

    private void OnDisable()
    {
        UnSetProperties();
    }

    protected  void SetProperties()
    {
        exitBtn = root.Q<Button>("ExitBtn");

        exitBtn.RegisterCallback<ClickEvent>(ExitSettingsMenu);
    }

    protected  void UnSetProperties()
    {
        exitBtn.UnregisterCallback<ClickEvent>(ExitSettingsMenu);
    }

   

   

    private void ExitSettingsMenu(ClickEvent evt)
    {
        Debug.Log("Settings menu closed");
        SceneTransitionManager.Instance.CloseAsyncScene("Settings");
    }

    private IEnumerator SetupUI() //Slight delay
    {
        yield return null;

        root = uiDocument.rootVisualElement;

        SetProperties();

      
    }
}

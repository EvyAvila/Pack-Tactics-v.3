using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.Device;

public enum SceneScript { HomeMenu, SettingsMenu, SaveMenu } 

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument uiDocument;

    [SerializeField]
    private List<Menus> menus;

    [SerializeField]
    private List<Menus> additionalMenus;

    private BaseMenu currentMenu;

    [SerializeField]
    private SceneScript startingUIScript;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LoadNextMenu(startingUIScript);

        foreach (var menu in additionalMenus)
        {
            menu.MenuScript.gameObject.SetActive(false);
        }
    }

    private void SwitchUIMenu(VisualTreeAsset screen, BaseMenu menu)
    {
        currentMenu?.Deactivate();

        uiDocument.rootVisualElement.Clear();
        if (screen != null)
        {
            uiDocument.visualTreeAsset = screen;
        }

        currentMenu = menu;
        currentMenu?.Activate(uiDocument);
    }


    public void LoadNextMenu(SceneScript scriptName)
    {
        int index = menus.FindIndex(x => x.MenuScript.scriptName == scriptName);
        SwitchUIMenu(menus[index].MenuAsset, menus[index].MenuScript);
    }

    public void DisplayMenuOnTop(SceneScript scriptName, bool conditions)
    {
        int index = additionalMenus.FindIndex(x => x.MenuScript.scriptName == scriptName);
        additionalMenus[index].MenuScript.gameObject.SetActive(conditions);

        if (conditions)
        {
            additionalMenus[index].MenuScript?.ShowCase();
        }
        else
        {
            additionalMenus[index].MenuScript?.HideCase();
        }
    }

}

[System.Serializable]
public class Menus
{
    public VisualTreeAsset MenuAsset;
    public BaseMenu MenuScript;
}

public abstract class BaseMenu : MonoBehaviour
{
    protected VisualElement root;

    public SceneScript scriptName { get; set; }

    public virtual void Activate(UIDocument document)
    {
        root = document.rootVisualElement;
        SetProperties();
    }

    public virtual void Deactivate()
    {
        UnSetProperties();
        root = null;
    }

    public virtual void ShowCase()
    {
        SetProperties();
    }

    public virtual void HideCase()
    {
        UnSetProperties();
    }

    protected abstract void SetProperties();
    protected abstract void UnSetProperties();

    protected abstract void Awake();

}
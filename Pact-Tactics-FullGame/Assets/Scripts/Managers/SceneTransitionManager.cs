using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public enum SceneType { MainMenu, Gameplay, Settings, Save }

public class SceneTransitionManager : MonoBehaviour
{
    //public static SceneTransitionManager Instance;

    [SerializeField]
    private List<SceneEntry> scenes;

    private Dictionary<SceneType, string> sceneMap;

    private void Awake()
    {
        sceneMap = scenes.ToDictionary(s => s.type, s => s.sceneName);
    }


    private void OnEnable()
    {
        CustomizedEventActions.OnRequestSceneLoad += UpdateScene;
        CustomizedEventActions.OnRequestUnloadScene += HideScene;
    }

    private void OnDisable()
    {
        CustomizedEventActions.OnRequestSceneLoad -= UpdateScene;
        CustomizedEventActions.OnRequestUnloadScene -= HideScene;
    }

    public void UpdateScene(SceneType sceneType)
    {
        if (sceneMap.TryGetValue(sceneType, out string sceneName))
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
        else
        {
            Debug.LogError("Scene not mapped: " + sceneType);
        }
    }

    public void HideScene(SceneType sceneType)
    {
        if (sceneMap.TryGetValue(sceneType, out string sceneName))
        {
            StartCoroutine(UnloadSceneCoroutine(sceneName));
        }
        else
        {
            Debug.LogError("Scene not mapped: " + sceneType);
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    private IEnumerator UnloadSceneCoroutine(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);
    }
}

[System.Serializable]
public class SceneEntry
{
    public SceneType type;
    public string sceneName;
}

public static class CustomizedEventActions
{
    public static Action<SceneType> OnRequestSceneLoad;

    public static Action<SceneType> OnRequestUnloadScene;
}

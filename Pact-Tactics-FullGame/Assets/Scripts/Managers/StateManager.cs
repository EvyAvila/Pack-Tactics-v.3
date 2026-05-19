using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public enum GameState { MainMenu, Game, Settings, Pause}

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }

    public GameState currentState { get; private set;}

    [SerializeField]
    private bool launchMenuOnStart = true;

    [SerializeField]
    private FadeUI fadeUIEffect;

    [SerializeField]
    private SceneTransitionManager sceneManager;


    private void Awake()
    {
       if(Instance == null)
        {
            Instance = this;
       }
       else
        {
            Destroy(gameObject);
            return;
       }
    }

    private void Start()
    {
        if(launchMenuOnStart)
        {
            ChangeState(GameState.MainMenu);
        }
    }

    public void ChangeState(GameState newState)
    {
        if(currentState != newState )
        {
            currentState = newState;
        }

        switch (currentState) 
        {
            case GameState.MainMenu:
                StartCoroutine(StateTransition(SceneType.MainMenu));
                break;

            case GameState.Game:
                StartCoroutine(StateTransition(SceneType.Gameplay));
                break;
        }

    }

    private IEnumerator StateTransition(SceneType state)
    {
        yield return fadeUIEffect.FadeIn();

        yield return sceneManager.ChangeScene(state);

        foreach (var s in sceneManager.sceneMap)
        {
            if (SceneManager.GetSceneByName(s.Value).isLoaded && s.Key != state)
            {
                yield return sceneManager.RemoveSceneCoroutine(s.Key);
            }
        }

        yield return fadeUIEffect.FadeOut();
    }

    

}
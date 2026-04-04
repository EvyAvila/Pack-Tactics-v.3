using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, Game, Settings, Pause}

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }

    public GameState currentState { get; private set;}

    [SerializeField]
    private bool launchMenuOnStart = true;


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
                CustomizedEventActions.OnRequestSceneLoad?.Invoke(SceneType.MainMenu);
                
                if(SceneManager.GetSceneByName("Game").isLoaded)
                {
                    CustomizedEventActions.OnRequestUnloadScene?.Invoke(SceneType.Gameplay);
                }
                break;

            case GameState.Game:
                CustomizedEventActions.OnRequestSceneLoad?.Invoke(SceneType.Gameplay);
                
                if (SceneManager.GetSceneByName("Home").isLoaded)
                {
                    CustomizedEventActions.OnRequestUnloadScene?.Invoke(SceneType.MainMenu);
                }
                break;
        }
    }
}
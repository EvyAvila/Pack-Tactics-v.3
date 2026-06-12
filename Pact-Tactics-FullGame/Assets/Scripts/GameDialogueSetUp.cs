using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Yarn.Unity;

public class GameDialogueSetUp : MonoBehaviour
{
    [SerializeField]
    private DialogueRunner dialogueRunner;

    [SerializeField]
    private int milliseconds = 3000;

    [SerializeField]
    private bool setTimer;

     
    void Start()
    {
        if (setTimer)
        {
            WaitTime();
        }
        
    }


    //TODO - Wait for the fade in to finish and then wait a second before
    //Potential idea: having a public bool that is set to true to being finished, apply function
    private async void WaitTime()
    {
        await Task.Delay(milliseconds);

        await dialogueRunner.StartDialogue(MainPlayer.instance.character.startingNode);
    }
}

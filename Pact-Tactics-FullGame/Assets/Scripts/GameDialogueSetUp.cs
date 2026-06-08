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

    //LEFT OFF
    void Start()
    {
        WaitTime();
    }


    private async void WaitTime()
    {
        await Task.Delay(milliseconds);

        await dialogueRunner.StartDialogue(MainPlayer.instance.character.startingNode);
    }
}

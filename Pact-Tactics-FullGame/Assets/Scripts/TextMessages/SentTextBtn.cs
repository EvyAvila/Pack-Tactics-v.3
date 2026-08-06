using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentTextBtn : MonoBehaviour
{
    public TextMeshProUGUI textMessageBox;
   
    public GameObject contextParent;

    [SerializeField]
    private GameObject playerBubble;

    [SerializeField]
    private Messages messages;

    private int currentPosition;

    private void Start()
    {
       currentPosition = 0;
    }



    public void SetText()
    {
        textMessageBox.text = messages.playerMessageResponse[currentPosition].message;
    }


    public void SendTextMessage()
    {
        if(currentPosition < messages.playerMessageResponse.Count)
        {
            playerBubble.GetComponentInChildren<TextMeshProUGUI>().text = messages.playerMessageResponse[currentPosition].message;
            var playerTextBubble = Instantiate(playerBubble, contextParent.transform);
            ++currentPosition;
        }
        

        textMessageBox.text = "";

        
        Debug.Log("Send button has been pressed");
    }
}

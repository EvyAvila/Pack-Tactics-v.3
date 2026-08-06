using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMessageManager : MonoBehaviour
{

    [SerializeField]
    private SentTextBtn sendTextBtn;

    [SerializeField]
    private BubbleMessage bubbleMessages;

    [SerializeField]
    private bool showUpdatedText;

    public List<GameObject> chatHistory;


    void Awake()
    {
        
    }

    private void Start()
    {
       
        bubbleMessages.UpdateChat(chatHistory);
        sendTextBtn.SetText();
        
    }

    private void Update()
    {
        //temp for testing purpose
        if(showUpdatedText)
        {
            //sendTextBtn.SendTextMessage();

            bubbleMessages.UpdateChat(chatHistory);

            showUpdatedText = false;

            sendTextBtn.SetText();

            
        }
    }

}

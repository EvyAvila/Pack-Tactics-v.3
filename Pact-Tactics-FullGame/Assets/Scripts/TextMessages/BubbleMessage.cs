using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class BubbleMessage : MonoBehaviour
{
    [SerializeField]
    private GameObject otherBubble;

    [SerializeField]
    private Messages messages;

    
    
    void Start()
    {
        
    }

    public void UpdateChat(List<GameObject> textBubbles)
    {
        for(int i = 0; i < messages.messagesList.Count; i++)
        {
            if(messages.messagesList[i].isSpeaking && !messages.messagesList[i].waitForResponse && !messages.messagesList[i].hasBeenAddedToChatHistory)
            {
                messages.messagesList[i].hasBeenAddedToChatHistory = true;
                otherBubble.GetComponentInChildren<TextMeshProUGUI>().text = messages.messagesList[i].message;
                var otherTextBubble = Instantiate(otherBubble, this.gameObject.transform);
                otherTextBubble.SetActive(true);
                textBubbles.Add(otherTextBubble);
                //textBubbles[i].SetActive(true);
            }
            else if (messages.messagesList[i].isSpeaking && messages.messagesList[i].waitForResponse && !messages.messagesList[i].hasBeenAddedToChatHistory)
            {
                messages.messagesList[i].hasBeenAddedToChatHistory = true;
                otherBubble.GetComponentInChildren<TextMeshProUGUI>().text = messages.messagesList[i].message;
                var otherTextBubble = Instantiate(otherBubble, this.gameObject.transform);
                otherTextBubble.SetActive(true);
                textBubbles.Add(otherTextBubble);
                //textBubbles[i].SetActive(true);
                break;
            }
        }
    }

    /* copy
           foreach (var m in messages.messagesList)
        {
            if (m.isPlayer)
            {
                //playerBubble.GetComponentInChildren<TextMeshProUGUI>().text = m.message;
                //var playerTextBubble = Instantiate(playerBubble, this.gameObject.transform);
                //textBubbles.Add(playerTextBubble);

                getMessage?.Invoke(m.message);
            }
            else
            {
                otherBubble.GetComponentInChildren<TextMeshProUGUI>().text = m.message;
                var otherTextBubble = Instantiate(otherBubble, this.gameObject.transform);
                otherTextBubble.SetActive(true);
                textBubbles.Add(otherTextBubble);
            }
        }
     
     */
}

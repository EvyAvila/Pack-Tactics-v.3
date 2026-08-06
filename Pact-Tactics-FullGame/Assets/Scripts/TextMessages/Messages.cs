using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messages : MonoBehaviour
{
    public List<MessageType> messagesList;

    public List<MessageType> playerMessageResponse;
}


[Serializable]
public class MessageType
{
    public string message;
    public bool isSpeaking, waitForResponse;
    public bool hasBeenAddedToChatHistory { get; set; }
}

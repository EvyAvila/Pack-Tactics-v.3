using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPCSO", order = 2)]
public class NPC : ScriptableObject, IEntity
{
    [SerializeField]
    private string character_Name;

    [SerializeField, TextArea(6, 14)]
    private string character_Bio;

    [SerializeField]
    private int character_age;

    [SerializeField]
    private int max_Trust, current_Trust;

    public string characterName { get { return character_Name; } set { character_Name = value; } }
    public string characterBio { get { return character_Bio; } set { character_Bio = value; } }
    public int characterAge { get { return character_age; } set { character_age = value; } }
    
    public int maxTrust { get { return max_Trust; } set { maxTrust= value; } }
    public int currentTrust { get { return current_Trust; } set { current_Trust= value; } }
}

[System.Serializable]
public class NPCSaveData
{
    public int current_Trust;

    public NPCSaveData(int cTrust) 
    { 
        current_Trust = cTrust;
    }    
}
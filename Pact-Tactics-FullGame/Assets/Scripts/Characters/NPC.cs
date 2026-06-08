using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPCSO", order = 1)]
public class NPC : ScriptableObject, IEntity
{
    [SerializeField]
    private string character_Name;

    [SerializeField, TextArea(6, 14)]
    private string character_Bio;

    [SerializeField]
    private int character_age;

    public string characterName { get { return character_Name; } set { character_Name = value; } }
    public string characterBio { get { return character_Bio; } set { character_Bio = value; } }
    public int characterAge { get { return character_age; } set { character_age = value; } }
}

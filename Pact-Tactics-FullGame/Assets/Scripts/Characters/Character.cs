using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterSO", order = 1)]
public class Character : ScriptableObject, IEntity
{
    [SerializeField]
    private string character_Name;

    [SerializeField]
    private string character_Type;

    [SerializeField, TextArea(6, 14)]
    private string character_Bio;


    [SerializeField]
    [Tooltip("Set the starting node for the yarn script, depending on which character is choosen.")]
    private string starting_Node;


    public string characterName { get { return character_Name; } set { character_Name = value; } }
    public string characterType { get { return character_Type; } set { character_Type = value; } }
    public string characterBio { get { return character_Bio; } set { character_Bio = value; } }

    public string startingNode { get { return starting_Node; } set { starting_Node = value; } }
}

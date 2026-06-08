using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    public static MainPlayer instance;

    [SerializeField]
    private Character mainCharacter;

    public Character character { get { return mainCharacter; } set { mainCharacter = value; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        //Implement later
        if(mainCharacter != null) 
        {
            //Load character from saved file
        }
    }

    public void SetCharacter(Character c)
    {
        mainCharacter = c;

        Debug.Log(mainCharacter.characterType + " has been selected and set");
    }
}

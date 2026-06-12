using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TrustSystemManager : MonoBehaviour
{
    [SerializeField]
    private DialogueRunner dialogueRunner;

    public List<NPC> npcList;

    [SerializeField]
    private bool resetCurrentTrustLevel;

    [SerializeField]
    private bool isAllowToEnterNegativeTrustLevel, isAllowToGoOverMaxTrustLevel;

    void Awake()
    {
        
    }


    [YarnCommand("StartPoint")] 
    public void StartPoint(string name) //Loading into the script
    {
        int pos = npcList.FindIndex(x => x.characterName == name);
        if(resetCurrentTrustLevel)
        {
            npcList[pos].currentTrust = 0;
            SaveVariables(pos);
        }
        else
        {
            LoadVariables(pos);
        }
        UpdateYarnVariables(pos);
    }

    [YarnCommand("Increase")]
    public void Increase(string name)
    {
        int pos = npcList.FindIndex(x => x.characterName == name);


        if(isAllowToGoOverMaxTrustLevel)
        {
            npcList[pos].currentTrust++;
        }
        else
        {
            npcList[pos].currentTrust++;
            if (npcList[pos].currentTrust < npcList[pos].maxTrust)
            {
                npcList[pos].currentTrust++;
            }
        }
        

        Debug.Log(npcList[pos].currentTrust);

        UpdateYarnVariables(pos);
    }

    [YarnCommand("Decrease")]
    public void Decrease(string name)
    {
        int pos = npcList.FindIndex(x => x.characterName == name);

        if(isAllowToEnterNegativeTrustLevel)
        {
            npcList[pos].currentTrust--;
        }
        else
        {
            npcList[pos].currentTrust--; 

            if (npcList[pos].currentTrust < 0)
            {
                npcList[pos].currentTrust = 0;
            }
        }
       
        Debug.Log(npcList[pos].currentTrust);

        UpdateYarnVariables(pos);
    }

    private void UpdateYarnVariables(int pos)
    {
        if(dialogueRunner != null && dialogueRunner.VariableStorage != null) 
        {
            dialogueRunner.VariableStorage.SetValue("$currentTrust", npcList[pos].currentTrust);
        }

        SaveVariables(pos);
    }


    private void SaveVariables(int pos)
    {
        NPCSaveData saveData = new NPCSaveData(npcList[pos].currentTrust);
        string jsonData = JsonUtility.ToJson(saveData);
        string saveKey = $"npc_{npcList[pos].characterName}";

        PlayerPrefs.SetString(saveKey, jsonData);
        PlayerPrefs.Save();
    }

    private void LoadVariables(int pos)
    {
        string saveKey = $"npc_{npcList[pos].characterName}";

        if(PlayerPrefs.HasKey(saveKey))
        {
            string jsonData = PlayerPrefs.GetString(saveKey);
            NPCSaveData saveData = JsonUtility.FromJson<NPCSaveData>(jsonData);

            npcList[pos].currentTrust = saveData.current_Trust;
        }
    }

    //Later save data, load, and reset/delete
}

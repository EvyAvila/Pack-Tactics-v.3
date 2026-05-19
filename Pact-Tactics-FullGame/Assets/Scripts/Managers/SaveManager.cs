using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private FontSizeEvent fonts;

    [SerializeField]
    private WindowControls window;

    [SerializeField]
    private AudioControls audios;

    [SerializeField]
    private bool resetPropertyValues;

    private void Start()
    {
        if (resetPropertyValues)
        {
            ResetPreference();
        }

        //LoadPreference();
        
    }
    public void SavePreference()
    {
        PlayerPrefs.SetFloat("FontSize", fonts.FontSize);
        
        PlayerPrefs.SetString("WindowSize", window.WindowSize);

        PlayerPrefs.SetInt("MusicPos", audios.MusicPos);
        PlayerPrefs.SetInt("SFXPos", audios.SFXPos);
        PlayerPrefs.SetInt("VAPos", audios.VAPos);

        PlayerPrefs.Save();
    }

    public void LoadPreference()
    {
        if (PlayerPrefs.HasKey("FontSize"))
        {
            fonts.FontSize = PlayerPrefs.GetFloat("FontSize");
            fonts.UpdateSizeValue(fonts.FontSize);
        }
        if(PlayerPrefs.HasKey("WindowSize"))
        {
            window.WindowSize = PlayerPrefs.GetString("WindowSize");
            window.LoadWindowSize(window.WindowSize);
        }
        if(PlayerPrefs.HasKey("MusicPos"))
        {
            audios.MusicPos = PlayerPrefs.GetInt("MusicPos");
            audios.LoadMusicList(audios.MusicPos);
        }
        if (PlayerPrefs.HasKey("SFXPos"))
        {
            audios.SFXPos = PlayerPrefs.GetInt("SFXPos");
            audios.LoadSFXList(audios.SFXPos);
        }
        if(PlayerPrefs.HasKey("VAPos"))
        {
            audios.VAPos = PlayerPrefs.GetInt("VAPos");
            audios.LoadVAList(audios.VAPos);
        }
    }

    void ResetPreference()
    {
        PlayerPrefs.SetFloat("FontSize", 40);
        PlayerPrefs.SetString("WindowSize", "full");

        PlayerPrefs.SetInt("MusicPos", 5);
        PlayerPrefs.SetInt("SFXPos", 5);
        PlayerPrefs.SetInt("VAPos", 5);

        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SavePreference();
    }
}

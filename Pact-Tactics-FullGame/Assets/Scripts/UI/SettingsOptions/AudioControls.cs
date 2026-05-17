using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Threading.Tasks;

public class AudioControls : Controls
{
    #region Data Fields
    [SerializeField]
    private string audioOptionMain, musicVolume, sfxVolume, voiceActingVolume;

    public string AudioOptionMain { get { return audioOptionMain; } set { audioOptionMain = value; } }
    public string MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
    public string SfxVolume { get { return sfxVolume; } set { sfxVolume = value; } }
    public string VoiceActingVolume { get { return voiceActingVolume; } set { voiceActingVolume = value; } }

    public List<Button> musicVolumeBtns = new List<Button>(); 
    
    private List<Button> sfxVolumeBtns = new List<Button>();

    private List<Button> voiceActVolumeBtns = new List<Button>();

    public List<ButtonVisuals> volumes;

    [SerializeField]
    private int milliseconds;

    private int musicPos, sfxPos, vaPos;

    public int MusicPos { get { return musicPos; } set { musicPos = value; } }
    public int SFXPos { get { return sfxPos; } set { sfxPos = value; } }
    public int VAPos { get { return vaPos; } set { vaPos = value; } }


    #endregion

    private void Start()
    {
        musicPos = musicPos == 0 ? 5 : musicPos;
        sfxPos = sfxPos == 0 ? 5 : sfxPos;
        vaPos = vaPos == 0 ? 5 : vaPos;
    }

    public override void ShowcaseOptions(VisualElement window)
    {
        base.ShowcaseOptions(window);
    }

    public void SetButtonLists(VisualElement music, VisualElement sfx, VisualElement va)
    {
        musicVolumeBtns = music.Query<Button>().ToList();
        sfxVolumeBtns = sfx.Query<Button>().ToList();
        voiceActVolumeBtns = va.Query<Button>().ToList();

        AssignMusicBtns(musicVolumeBtns);
        AssignMusicBtns(sfxVolumeBtns);
        AssignMusicBtns(voiceActVolumeBtns);
    }
    public void UnsetButtonsList()
    {
        UnassignMusicBtns(musicVolumeBtns);
        UnassignMusicBtns(sfxVolumeBtns);
        UnassignMusicBtns(voiceActVolumeBtns);
    }

    
    private void AssignMusicBtns(List<Button> volumeBtns)
    {
        for (int i = 0; i < volumeBtns.Count; i++)
        {
            volumeBtns[i].userData = i;
            volumeBtns[i].RegisterCallback<ClickEvent>(VolumbeBtnControl);
        }
    }

    private void UnassignMusicBtns(List<Button> volumeBtns)
    {
        foreach (var volume in volumeBtns)
        {
            volume.UnregisterCallback<ClickEvent>(VolumbeBtnControl);
        }
    }

    private void VolumbeBtnControl(ClickEvent evt)
    {
        Button btn = evt.currentTarget as Button;

        int index = (int)btn.userData;

        //Debug.Log($"Clicked button {index}: {btn.name}");

        UpdateButtonList(btn, index);
    }

    private void UpdateButtonList(Button btn, int index)
    {
        if (btn.parent.name == musicVolume)
        {
            GetVolumeList(index, musicVolumeBtns, true);
            AudioManager.instance.SetMusicVolume(index);
        }
        if(btn.parent.name == sfxVolume)
        {
            GetVolumeList(index, sfxVolumeBtns, true);
            AudioManager.instance.SetSFXVolume(index);
        }
        if(btn.parent.name == voiceActingVolume)
        {
            GetVolumeList(index, voiceActVolumeBtns, true);
            AudioManager.instance.SetVAVolume(index);
        }
    }

    public void LoadMusicList(int index)
    {
        GetVolumeList(index, musicVolumeBtns, false);
        AudioManager.instance.SetMusicVolume(index);
    }    
    
    public void LoadSFXList(int index)
    {
        GetVolumeList(index, sfxVolumeBtns, false);
        AudioManager.instance.SetSFXVolume(index);
    }    
    
    public void LoadVAList(int index)
    {
        GetVolumeList(index, voiceActVolumeBtns, false);
        AudioManager.instance.SetVAVolume(index);
    }

    private void GetVolumeList(int index, List<Button> volumeBtnList, bool canAnimate)
    {
        //Reset the volume to be empty before applying the animation again
        for(int i = 0; i < volumeBtnList.Count; i++)
        {
            volumeBtnList[i].style.backgroundImage = volumes[i].volumeEmpty;
        }

        //Start setting correct position
        if(volumeBtnList == musicVolumeBtns)
        {
            musicPos = index;
        }
        if(volumeBtnList == sfxVolumeBtns)
        {
            sfxPos = index;
        }
        if(volumeBtnList == voiceActVolumeBtns)
        {
            vaPos = index;
        }

        SetVolume(index, volumeBtnList, canAnimate);
    }

    public async void SetVolume(int index, List<Button> volumeButtons, bool canAnimate)
    {
        for (int i = 0; i < volumeButtons.Count; i++)
        {
            if (i <= index)
            {
                volumeButtons[i].style.backgroundImage = volumes[i].volumeFull;
            }
            else
            {
                volumeButtons[i].style.backgroundImage = volumes[i].volumeEmpty;
            }

            if(canAnimate)
            {
                await Task.Delay(milliseconds);
            }
        }
    }
}

[System.Serializable]
public class ButtonVisuals
{
    public Texture2D volumeEmpty;
    public Texture2D volumeFull;
}

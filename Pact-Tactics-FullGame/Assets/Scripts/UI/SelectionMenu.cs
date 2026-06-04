using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class SelectionMenu : AbstractMenuBase
{
    private Button leftBtn, rightBtn, selectBtn;

    private Label playerType, playerBio;

    [SerializeField]
    private List<Character> characterTypes;

    [SerializeField]
    private string playerTypeTxt, playerBioTxt, leftBtnTxt, rightBtnTxt, selectBtnTxt;

    private int listPosition;

    public override void SetProperties()
    {
        SetButtons();
        SetLables();
    }

    public override void UnSetProperties()
    {
        leftBtn.UnregisterCallback<ClickEvent>(SelectLeftButton);
        rightBtn.UnregisterCallback<ClickEvent>(SelectRightButton);
        selectBtn.UnregisterCallback<ClickEvent>(SelectConfirmationButton);
    }

    private void SetButtons()
    {
        leftBtn = root.Q<Button>(leftBtnTxt);
        leftBtn.RegisterCallback<ClickEvent>(SelectLeftButton);

        rightBtn = root.Q<Button>(rightBtnTxt);
        rightBtn.RegisterCallback<ClickEvent>(SelectRightButton);
        
        selectBtn = root.Q<Button>(selectBtnTxt);
        selectBtn.RegisterCallback<ClickEvent>(SelectConfirmationButton);

        SetEnabledButtons(true);
    }

    private void SetLables()
    {
        playerType = root.Q<Label>(playerTypeTxt);
        playerBio = root.Q<Label>(playerBioTxt);

        UpdateDisplay(0);
    }

    private void SelectLeftButton(ClickEvent evt)
    {
        UpdateDisplay(-1);
    }

    private void SelectRightButton(ClickEvent evt)
    {
        UpdateDisplay(1);
    }

    private void SelectConfirmationButton(ClickEvent evt)
    {
        MainPlayer.instance.SetCharacter(characterTypes[listPosition]);
        StateManager.Instance.ChangeState(GameState.Game);

        SetEnabledButtons(false);
    }

    private void SetEnabledButtons(bool condition)
    {
        selectBtn.SetEnabled(condition);
        leftBtn.SetEnabled(condition);
        rightBtn.SetEnabled(condition);

    }

    private void UpdateDisplay(int position)
    {
        listPosition += position;

        if(listPosition < 0) //If going left, reset it back to the last element of the list
        {
            listPosition = characterTypes.Count() - 1;
        }
        else if(listPosition > characterTypes.Count() - 1 || listPosition == 0) //If going right and reaches max (or when the scene loads) go to the first element of the list
        {
            listPosition = 0;
        }

        playerType.text = characterTypes[listPosition].characterType;
        playerBio.text = characterTypes[listPosition].characterBio;
    }
}

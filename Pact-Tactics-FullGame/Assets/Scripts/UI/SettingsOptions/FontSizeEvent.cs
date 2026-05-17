using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class FontSizeEvent : MonoBehaviour
{
    public TextMeshProUGUI optionItemPrefab;

    public OptionsPresenter presenter;

    public List<TextMeshProUGUI> mainText;
    
    [SerializeField]
    private float fontSize;
    
    public float FontSize { get { return fontSize; } set { fontSize = value; } }
    
    private void Start()
    {
        if (mainText == null)
        {
            mainText = new List<TextMeshProUGUI>();
        }

        if (presenter == null)
        {
            presenter = new OptionsPresenter();
        }

        UpdateSizeValue(fontSize);
    }

    public void UpdateSizeValue(float size)
    {
        fontSize = size;

        optionItemPrefab.fontSize = size;

        foreach (var t in mainText)
        {
            t.fontSize = size;
        }

        foreach (var v in presenter.OptionViews)
        {
            v.GetComponent<TextMeshProUGUI>().fontSize = size;
        }
    }
}

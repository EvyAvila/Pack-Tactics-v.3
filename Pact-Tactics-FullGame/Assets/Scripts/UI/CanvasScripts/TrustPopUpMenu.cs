using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum trustState { Increase, Decrease }

public class TrustPopUpMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TextMeshProUGUI trustText;
    
    [SerializeField]
    int milliseconds;

    void Start()
    {
        milliseconds = milliseconds == 0 ? 2000 : milliseconds;

        panel.SetActive(false);
    }

    private void ShowTrustCanvasUpdater()
    {
        panel.SetActive(true);
        stallDeactivation();
    }

    private async void stallDeactivation()
    {
        await Task.Delay(milliseconds);

        panel.SetActive(false);
    }

    public void SetText(trustState state)
    {
        switch (state)
        {
            case trustState.Increase:
                trustText.text = "Trust increased by 1";
                break;
            case trustState.Decrease:
                trustText.text = "Trust decreased by 1";
                break;
        }

        ShowTrustCanvasUpdater();
    }
    
    
}

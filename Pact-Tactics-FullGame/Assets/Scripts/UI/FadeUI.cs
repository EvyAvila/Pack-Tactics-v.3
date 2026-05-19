using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration;

    public float FadeDuration
    {
        get
        {
            return fadeDuration;
        }
        set
        {
            fadeDuration = value;
        }
    }

    [SerializeField]
    private Image fade;

    public bool canPlayEffect;

    private void Awake()
    {
        if (fade == null)
        {
            fade = GetComponent<Image>();
        }

        fade.color = new Color(0, 0, 0, 0);

        fadeDuration = fadeDuration == 0 ? 2 : fadeDuration;
    }

    public IEnumerator FadeIn()
    {
        Color startColor = new Color(0, 0, 0, 0);
        Color targetColor = new Color(0, 0, 0, 1);

        yield return FadeCoroutine(startColor, targetColor);
    }

    public IEnumerator FadeOut()
    {
        Color startColor = new Color(0, 0, 0, 1);
        Color targetColor = new Color(0, 0, 0, 0);

        yield return FadeCoroutine(startColor, targetColor);

    }

    private IEnumerator FadeCoroutine(Color startColor, Color targetColor)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while(elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / fadeDuration;
            fade.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }

}

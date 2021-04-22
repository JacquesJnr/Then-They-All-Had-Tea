using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ClickToPlay : MonoBehaviour
{
    public CanvasGroup storyBeats;
    public RectTransform lavaSpirit, forestSpirit;
    public CanvasGroup playText;
    public UnityEvent onClick;

    private DayNight timeCycle;
    private Territory territory;

    private void Awake()
    {
        lavaSpirit.LeanAlpha(0, 0);
        forestSpirit.LeanAlpha(0, 0);
        storyBeats.alpha = 0;
        
        timeCycle = FindObjectOfType<DayNight>();
        territory = FindObjectOfType<Territory>();

        timeCycle.enabled = false;
        territory.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClick.Invoke();
        }
    }

    public void FadeIn()
    {
        lavaSpirit.LeanAlpha(1, 1.5f).setEase(LeanTweenType.easeOutSine);
        forestSpirit.LeanAlpha(1, 1.5f).setEase(LeanTweenType.easeOutSine);

        storyBeats.LeanAlpha(1, 2f).setEase(LeanTweenType.easeOutSine);

        playText.LeanAlpha(0, 0.5f).setEase(LeanTweenType.easeInSine);
        timeCycle.enabled = true;
        territory.enabled = true;
        this.enabled = false;
    }
}

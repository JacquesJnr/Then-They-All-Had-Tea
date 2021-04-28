using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ClickToPlay : MonoBehaviour
{
    //public CanvasGroup storyBeats;
    public RectTransform lavaSpirit, forestSpirit, earthSpirit, skySpirit;
    public CanvasGroup playText;
    public UnityEvent onClick;

    private PushPullCycle timeCycle;
    private Territory territory;

    private void Start()
    {
        lavaSpirit.LeanAlpha(0, 0);
        forestSpirit.LeanAlpha(0, 0);
        earthSpirit.LeanAlpha(0, 0);
        skySpirit.LeanAlpha(0, 0);
        //storyBeats.alpha = 0;
        
        timeCycle = FindObjectOfType<PushPullCycle>();
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
        earthSpirit.LeanAlpha(1, 1.5f).setEase(LeanTweenType.easeOutSine);
        skySpirit.LeanAlpha(1, 1.5f).setEase(LeanTweenType.easeOutSine);

        //storyBeats.LeanAlpha(1, 2f).setEase(LeanTweenType.easeOutSine);

        playText.LeanAlpha(0, 0.5f).setEase(LeanTweenType.easeInSine);
        timeCycle.enabled = true;
        territory.enabled = true;
        this.enabled = false;
    }
}

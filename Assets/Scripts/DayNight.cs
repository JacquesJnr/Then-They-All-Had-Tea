using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour
{
    [SerializeField] public const float timer = 5f;
    [Range(0, timer)] public float elapsed = 0;
    [SerializeField] private CanvasGroup day_bg, night_bg;
    public bool isDay = true;

    private Territory territory;

    void Awake()
    {
        territory = FindObjectOfType<Territory>();
        night_bg.alpha = 0;
        day_bg.alpha = 1;
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= timer)
        {
            // Fade between backgrounds.
            FadeInOut(isDay);

            if (isDay)
                isDay = false;
            else
                isDay = true;

            territory.DayTime.Invoke();
            elapsed = 0;
        }
    }

    public void FadeInOut(bool isDay)
    {
        if (isDay) 
        {
            day_bg.LeanAlpha(0, 0.5f); // Fade day sky out
            night_bg.LeanAlpha(1, 0.5f); // Fade night sky in
            
        }
        else 
        {
            day_bg.LeanAlpha(1, 0.5f); // Fade day sky in
            night_bg.LeanAlpha(0, 0.5f); // Fade night sky out
        }
            
    }
}
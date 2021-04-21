using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour
{
    [SerializeField] public const float timer = 5f;
    [Range(0, timer)] public float elapsed = 0;
    [SerializeField] private CanvasGroup BG;
    public bool isDay = true;

    private Territory territory;

    void Awake()
    {
        territory = FindObjectOfType<Territory>();
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
        if(isDay)
            BG.LeanAlpha(0, 0.5f); // Fade to night sky.
        else
            BG.LeanAlpha(1, 0.5f); // Fade to day sky.
    }
}
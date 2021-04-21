using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour
{
    const float timer = 1f;
    [Range(0, timer)] public float elapsed = 0;
    [SerializeField] private CanvasGroup BG;
    [SerializeField] private bool dayTime = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= timer)
        {
            FadeInOut(dayTime);
            Debug.Log("Time");
            if (dayTime)
                dayTime = false;
            else
                dayTime = true;
            elapsed = 0;
        }
    }

    public void FadeInOut(bool isDay)
    {
        if(isDay)
            BG.LeanAlpha(0, 0.5f);
        else
            BG.LeanAlpha(1, 0.5f);
    }
}

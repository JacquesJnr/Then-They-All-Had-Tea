using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Territory : MonoBehaviour
{
    [SerializeField] private Image redProgressBar;
    [SerializeField] private Image greenProgressBar;
    [SerializeField] private RectTransform treeSpiritRect;
    [SerializeField] private RectTransform lavaSpiritRect;
    [SerializeField] private LeanTweenType buttonEase;

    public float greenMaximum = 0;
    public float redMaximum = 0;

    private const float territoryMin = 0.12f;
    private const float territoryMax = 0.87f;
    private const float territoryDelta = 0.76f;

    int clicksToPass = 10;

    public UnityEvent DayTime;
    private DayNight dayNight;

    private void Awake()
    {
        dayNight = FindObjectOfType<DayNight>();
    }

    private void Start()
    {
        // Set each territory to it's minimum value
        redProgressBar.fillAmount = territoryMin;
        greenProgressBar.fillAmount = territoryMin;

        // Set buttons to initial size
        treeSpiritRect.localScale = new Vector3(0.5f, 0.5f, 0);
        lavaSpiritRect.localScale = new Vector3(0.5f, 0.5f, 0);
    }

    private void Update()
    {
        if (dayNight.isDay)
        {
            // Grow Lava
            //Debug.Log("Day Time!");
            //StartCoroutine("test");
            if (redProgressBar.fillAmount < territoryMax)
                redProgressBar.fillAmount += territoryDelta / clicksToPass * Time.deltaTime; // * clickCounter

            //Decay Forest
            if (greenProgressBar.fillAmount > territoryMin)
                greenProgressBar.fillAmount -= territoryDelta / clicksToPass * Time.deltaTime;
        }

        else if (!dayNight.isDay)
        {
            // Grow Forest
            //Debug.Log("Night Time!");
            if (greenProgressBar.fillAmount < territoryMax)
                greenProgressBar.fillAmount += territoryDelta / clicksToPass * Time.deltaTime;

            // Decay Lava
            if (redProgressBar.fillAmount > territoryMin)
                redProgressBar.fillAmount -= territoryDelta / clicksToPass * Time.deltaTime;
        }
    }

    public void SpiritButton(RectTransform spirit)
    {
        var scaleX = spirit.localScale.x;
        var scaleY = spirit.localScale.y;
        float scaleDiff = 0.5f / clicksToPass;

        Vector3 newScale = new Vector3(scaleX + scaleDiff, scaleY + scaleDiff ,0);

        if (scaleX < 1)
        {
            spirit.LeanScale(newScale, 0.8f).setEase(buttonEase);
        }

        //if(spirit == treeSpiritRect)
        //{
        //    if (greenProgressBar.fillAmount < 0.87f)
        //        greenProgressBar.fillAmount += 0.76f / clicksToPass;
        //}
        //else
        //{
        //    if (redProgressBar.fillAmount < 0.87f)
        //        redProgressBar.fillAmount += 0.76f / clicksToPass;
        //}        
    }


    // TO DO - Add click counter for each button
    public void GrowAndDecay()
    {
        // Check the isDay bool from the Day / Night script
        if (dayNight.isDay)
        {
            // Grow Lava
            //Debug.Log("Day Time!");
            //StartCoroutine("test");
            //if (redProgressBar.fillAmount < territoryMax)
            //    redProgressBar.fillAmount += territoryDelta / clicksToPass; // * clickCounter

            // Decay Forest
            //if (greenProgressBar.fillAmount > territoryMin)
            //    greenProgressBar.fillAmount -= territoryDelta / clicksToPass;
        }

        else if (!dayNight.isDay)
        {
            // Grow Forest
            //Debug.Log("Night Time!");
            //if (greenProgressBar.fillAmount < territoryMax)
            //    greenProgressBar.fillAmount += territoryDelta / clicksToPass;

            //// Decay Lava
            //if (redProgressBar.fillAmount > territoryMin)
            //    redProgressBar.fillAmount -= territoryDelta / clicksToPass;
        }
    }

    IEnumerator test()
    {
        //if (redProgressBar.fillAmount < territoryMax)
        //    redProgressBar.fillAmount += territoryDelta / clicksToPass;

        //while (greenProgressBar.fillAmount > territoryMin)
        //    greenProgressBar.fillAmount -= territoryDelta / clicksToPass * Time.deltaTime;

        yield return null;
    }
}

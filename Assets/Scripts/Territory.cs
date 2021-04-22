using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Territory : MonoBehaviour
{
    [SerializeField] private Image redProgressBar;
    [SerializeField] private Image greenProgressBar;
    [SerializeField] private RectTransform forestSpiritRect;
    [SerializeField] private RectTransform lavaSpiritRect;
    [SerializeField] private LeanTweenType buttonEase;

    private const float territoryMin = 0.12f;
    private const float territoryMax = 0.87f;
    private const float territoryDelta = territoryMax - territoryMin;

    int clicksToPass = 20;
    [SerializeField] private float forestClickCounter = 1;
    [SerializeField] private float lavaClickCounter = 1;

    public UnityEvent DawnDusk;
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
        forestSpiritRect.localScale = new Vector3(0.5f, 0.5f, 0);
        lavaSpiritRect.localScale = new Vector3(0.5f, 0.5f, 0);
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
        
        if(spirit == forestSpiritRect && forestClickCounter < 10)
        {
            forestClickCounter += 1;
        }

        if(spirit == lavaSpiritRect && lavaClickCounter < 10)
        {
            lavaClickCounter += 1;
        }

        spirit.gameObject.GetComponent<Button>().interactable = false;
    }

    public void GrowAndDecay()
    {
        var forestX = forestSpiritRect.localScale.x;
        var lavaX = lavaSpiritRect.localScale.x;
        var forestY = forestSpiritRect.localScale.y;
        var lavaY = forestSpiritRect.localScale.y;

        if (!forestSpiritRect.gameObject.GetComponent<Button>().interactable)
        {
            forestSpiritRect.gameObject.GetComponent<Button>().interactable = true;
        }

        if (!lavaSpiritRect.gameObject.GetComponent<Button>().interactable)
        {
            lavaSpiritRect.gameObject.GetComponent<Button>().interactable = true;
        }

        // Check the isDay bool from the Day / Night script
        if (dayNight.isDay)
        {
            // Grow Lava
            Debug.Log("Day Time!");
            if (redProgressBar.fillAmount < territoryMax)
                redProgressBar.fillAmount += (territoryDelta / clicksToPass) * lavaClickCounter;

            // Decay Forest
            if (greenProgressBar.fillAmount > territoryMin) 
            {
                greenProgressBar.fillAmount -= territoryDelta / clicksToPass;
                
                if (forestClickCounter > 1)
                    forestClickCounter -= 0.5f;
            }

            // Check the bars are touching
            if (greenProgressBar.fillAmount + redProgressBar.fillAmount >= 1)
            {
                // Equalise the progress bars
                greenProgressBar.fillAmount = 1 - redProgressBar.fillAmount;

                // Decrease the size of the forest spirit
                forestSpiritRect.LeanScale(new Vector3(forestX - (0.5f / clicksToPass), forestY - (0.5f / clicksToPass), 0), 0.8f).setEase(buttonEase);
            }
        }

        else if (!dayNight.isDay)
        {
            // Grow Forest
            Debug.Log("Night Time!");
            if (greenProgressBar.fillAmount < territoryMax)
                greenProgressBar.fillAmount += territoryDelta / clicksToPass * forestClickCounter;

            // Decay Lava
            if (redProgressBar.fillAmount > territoryMin) 
            {
                redProgressBar.fillAmount -= territoryDelta / clicksToPass;
                
                if(lavaClickCounter > 1)
                    lavaClickCounter -= 0.5f;             
            }

            // Check the bars are touching
            if (greenProgressBar.fillAmount + redProgressBar.fillAmount >= 1)
            {
                // Equalise the progress bars
                redProgressBar.fillAmount = 1 - greenProgressBar.fillAmount;

                // Decrease the size of the lava spirit
                lavaSpiritRect.LeanScale(new Vector3(lavaX - (0.5f / clicksToPass), lavaY - (0.5f / clicksToPass), 0), 0.8f).setEase(buttonEase);
            }
        }
    }
}
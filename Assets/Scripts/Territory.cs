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
    [SerializeField] private CanvasGroup waterNeutral, waterLeafy, waterHot;
    [SerializeField] private LeanTweenType buttonEase;

    private const float territoryMin = 0.12f;
    private const float territoryMax = 0.87f;
    private const float territoryDelta = territoryMax - territoryMin;

    int clicksToPass = 20;
    [SerializeField] private float forestClickCounter = 1;
    [SerializeField] private float lavaClickCounter = 1;

    public UnityEvent DawnDusk;
    private DayNight dayNight;

    public bool isTouching;

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

    private void Update()
    {
        GrowAndDecay();

        float forestScaler = ((0.5f * greenProgressBar.fillAmount) / territoryDelta) + 0.25f;
        float lavaScaler = ((0.5f * redProgressBar.fillAmount) / territoryDelta) + 0.25f;

        Vector3 forestButtonScale = new Vector3(forestScaler, forestScaler, 0);
        Vector3 lavaButtonScale = new Vector3(lavaScaler, lavaScaler, 0);

        if (isTouching)
        {           
            forestSpiritRect.LeanScale(forestButtonScale, 0.8f).setEase(LeanTweenType.easeInSine);
            lavaSpiritRect.LeanScale(lavaButtonScale, 0.8f).setEase(LeanTweenType.easeInSine);
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

    public void EnableButton()
    {

        if (!forestSpiritRect.gameObject.GetComponent<Button>().interactable)
            forestSpiritRect.gameObject.GetComponent<Button>().interactable = true;

        if (forestClickCounter > 1)
            forestClickCounter -= 0.5f;

        if (!lavaSpiritRect.gameObject.GetComponent<Button>().interactable)
            lavaSpiritRect.gameObject.GetComponent<Button>().interactable = true;

        if (lavaClickCounter > 1)
            lavaClickCounter -= 0.5f;

        // Check the lava teritory is past the water
        if (redProgressBar.fillAmount > 0.62f)
        {
            waterNeutral.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
            waterLeafy.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
            waterHot.LeanAlpha(1, 0.8f).setEase(LeanTweenType.easeInOutSine);
        }

        // Check the forest teritory is past the water
        if (greenProgressBar.fillAmount > 0.62f)
        {
            waterNeutral.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
            waterHot.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
            waterLeafy.LeanAlpha(1, 0.8f).setEase(LeanTweenType.easeInOutSine);
        }
    }

    public void GrowAndDecay()
    {
        var forestX = forestSpiritRect.localScale.x;
        var lavaX = lavaSpiritRect.localScale.x;
        var forestY = forestSpiritRect.localScale.y;
        var lavaY = lavaSpiritRect.localScale.y;

        Vector3 smallForest = new Vector3(1 - lavaX, 1 - lavaY, 0);
        Vector3 smallLava = new Vector3(1 - forestX, 1 - forestY, 0);


        // Check the isDay bool from the Day / Night script
        if (dayNight.isDay)
        {
            // Grow Lava
            Debug.Log("Day Time!");
            if (redProgressBar.fillAmount < territoryMax)
                redProgressBar.fillAmount += ((territoryDelta / clicksToPass) * lavaClickCounter) * Time.deltaTime;

            // Decay Forest
            if (greenProgressBar.fillAmount > territoryMin) 
                greenProgressBar.fillAmount -= (territoryDelta / clicksToPass) * Time.deltaTime;               

            // Check the bars are touching
            if (greenProgressBar.fillAmount + redProgressBar.fillAmount >= 1)
            {
                isTouching = true;

                // Equalise the progress bars
                greenProgressBar.fillAmount = 1 - redProgressBar.fillAmount;
            }
        }

        else if (!dayNight.isDay)
        {
            // Grow Forest
            Debug.Log("Night Time!");
            if (greenProgressBar.fillAmount < territoryMax)
                greenProgressBar.fillAmount += (territoryDelta / clicksToPass * forestClickCounter) * Time.deltaTime;

            // Decay Lava
            if (redProgressBar.fillAmount > territoryMin) 
                redProgressBar.fillAmount -= (territoryDelta / clicksToPass) * Time.deltaTime;

            // Check the bars are touching
            if (greenProgressBar.fillAmount + redProgressBar.fillAmount >= 1)
            {
                isTouching = true;

                // Equalise the progress bars
                redProgressBar.fillAmount = 1 - greenProgressBar.fillAmount;             
            }          
        }
    }
}

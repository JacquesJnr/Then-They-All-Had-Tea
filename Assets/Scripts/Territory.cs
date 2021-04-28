using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Territory : MonoBehaviour
{
    [SerializeField] private Image lavaMeter;
    [SerializeField] private Image forestMeter;

    [SerializeField] private RectTransform theEarth;
    [SerializeField] private RectTransform theSky;

    [SerializeField] private RectTransform forest;
    [SerializeField] private RectTransform skySpirit;

    [SerializeField] private RectTransform lava;
    [SerializeField] private RectTransform earthSpirit;

    [SerializeField] private CanvasGroup waterNeutral, waterLeafy, waterHot;
    [SerializeField] private LeanTweenType buttonEase;

    private const float territoryMin = 0.2f;
    private const float territoryMax = 1.0f;
    private const float territoryDelta = territoryMax - territoryMin;
    private const float earthMax = 2.98f;
    private const float earthMin = 0;
    private const float skyMax = -2.98f;
    private const float skyMin = 0;

    int clicksToPass = 20;
    [SerializeField] private float forestClickCounter = 1;
    [SerializeField] private float lavaClickCounter = 1;

    public UnityEvent GrowDecayCycle;
    private PushPullCycle pushPull;
    private ClickToPlay menuScript;

    public bool isTouching;

    private void Awake()
    {
        menuScript = FindObjectOfType<ClickToPlay>();        
        pushPull = FindObjectOfType<PushPullCycle>();
    }

    private void Start()
    {
        // Set each territory to it's minimum value
        lavaMeter.fillAmount = territoryMin;
        forestMeter.fillAmount = territoryMin;

        // Set buttons to initial size
        forest.localScale = new Vector3(0.5f, 0.5f, 0);
        lava.localScale = new Vector3(0.5f, 0.5f, 0);
        earthSpirit.localScale = new Vector3(0.5f, 0.5f, 0);
        skySpirit.localScale = new Vector3(0.5f, 0.5f, 0);

        if (!menuScript.enabled)
            menuScript.playText.alpha = 0;
        else
            menuScript.playText.alpha = 1;
    }

    private void Update()
    {
        GrowAndDecay();
        //Debug.Log(earth.transform.localPosition);

        float forestScaler = ((0.5f * forestMeter.fillAmount) / territoryDelta) + 0.25f;
        float lavaScaler = ((0.5f * lavaMeter.fillAmount) / territoryDelta) + 0.25f;

        Vector3 forestButtonScale = new Vector3(forestScaler, forestScaler, 0);
        Vector3 lavaButtonScale = new Vector3(lavaScaler, lavaScaler, 0);

        if (isTouching)
        {           
            forest.LeanScale(forestButtonScale, 0.8f).setEase(LeanTweenType.easeInSine);
            lava.LeanScale(lavaButtonScale, 0.8f).setEase(LeanTweenType.easeInSine);
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
        
        if(spirit == forest && forestClickCounter < 10)
        {
            forestClickCounter += 1;
        }

        if(spirit == lava && lavaClickCounter < 10)
        {
            lavaClickCounter += 1;
        }

        spirit.gameObject.GetComponent<Button>().interactable = false;
    }

    public void EnableButton()
    {

        if (!forest.gameObject.GetComponent<Button>().interactable)
            forest.gameObject.GetComponent<Button>().interactable = true;

        if (forestClickCounter > 1)
            forestClickCounter -= 0.5f;

        if (!lava.gameObject.GetComponent<Button>().interactable)
            lava.gameObject.GetComponent<Button>().interactable = true;

        if (lavaClickCounter > 1)
            lavaClickCounter -= 0.5f;

        if (!earthSpirit.gameObject.GetComponent<Button>().interactable)
            earthSpirit.gameObject.GetComponent<Button>().interactable = true;

        if (!skySpirit.gameObject.GetComponent<Button>().interactable)
            skySpirit.gameObject.GetComponent<Button>().interactable = true;

        // WATER TRANSISTIONS

        // Check the lava teritory is past the water
        //if (redProgressBar.fillAmount > 0.62f)
        //{
        //    waterNeutral.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
        //    waterLeafy.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
        //    waterHot.LeanAlpha(1, 0.8f).setEase(LeanTweenType.easeInOutSine);
        //}

        // Check the forest teritory is past the water
        //if (greenProgressBar.fillAmount > 0.62f)
        //{
        //    waterNeutral.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
        //    waterHot.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
        //    waterLeafy.LeanAlpha(1, 0.8f).setEase(LeanTweenType.easeInOutSine);
        //}
    }

    public void GrowAndDecay()
    {
        var forestX = forest.localScale.x;
        var lavaX = lava.localScale.x;
        var forestY = forest.localScale.y;
        var lavaY = lava.localScale.y;

        Vector3 landVector = theEarth.transform.position;
        Vector3 skyVector = theSky.transform.position;
        float step = 2 * Time.deltaTime;


        Vector3 smallForest = new Vector3(1 - lavaX, 1 - lavaY, 0);
        Vector3 smallLava = new Vector3(1 - forestX, 1 - forestY, 0);



        // Check the Push bool from the Day / Night script
        if (pushPull.Push)
        {
            Debug.Log("Push!");

            // Grow Lava
            if (lavaMeter.fillAmount < territoryMax)
                lavaMeter.fillAmount += ((territoryDelta / clicksToPass) * lavaClickCounter) * Time.deltaTime;

            // Grow Earth
            //if (theEarth.transform.position.y < earthMax)
            //{
            //    landVector = new Vector3(0, step, 0);
            //    theEarth.transform.position += landVector;
            //}

            // Decay Forest
            if (forestMeter.fillAmount > territoryMin) 
                forestMeter.fillAmount -= (territoryDelta / clicksToPass) * Time.deltaTime;       
            
            // Decay Sky
            if(theSky.transform.position.y < skyMin)
            {
                skyVector = new Vector3(0, step, 0);
                theSky.transform.position += skyVector;
            }   
            
            // Check the bars are touching
            if (forestMeter.fillAmount + lavaMeter.fillAmount >= 1)
            {
                isTouching = true;

                // Equalise the progress bars
                forestMeter.fillAmount = 1 - lavaMeter.fillAmount;
            }
        }

        else if (!pushPull.Push)
        {
            Debug.Log("Pull!");

            // Grow Forest
            if (forestMeter.fillAmount < territoryMax)
                forestMeter.fillAmount += (territoryDelta / clicksToPass * forestClickCounter) * Time.deltaTime;

            // Grow Sky
            if (theSky.transform.position.y > skyMax)
            {
                skyVector = new Vector3(0, step, 0);
                theSky.transform.position -= skyVector;
            }

            // Decay Lava
            if (lavaMeter.fillAmount > territoryMin) 
                lavaMeter.fillAmount -= (territoryDelta / clicksToPass) * Time.deltaTime;

            // Decay Earth
            if (theEarth.transform.position.y > earthMin)
            {
                landVector = new Vector3(0, step, 0);
                theEarth.transform.position -= landVector;
            }

            // Check the bars are touching
            if (forestMeter.fillAmount + lavaMeter.fillAmount >= 1)
            {
                isTouching = true;

                // Equalise the progress bars
                lavaMeter.fillAmount = 1 - forestMeter.fillAmount;             
            }          
        }
    }
}

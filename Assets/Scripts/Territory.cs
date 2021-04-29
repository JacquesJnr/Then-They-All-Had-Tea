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

    [SerializeField] private CanvasGroup neutral, leafy, hot, tea;
    [SerializeField] private CanvasGroup hurricanPopup, thunderPopup, windPopup, mudPopup;
    [SerializeField] private GameObject weatherPopups;

    [SerializeField] private GameObject closeButton;
    private Sprite closeButtonSprite;
    
    [SerializeField] private Sprite closeButtonBlue;
    [SerializeField] private Sprite closeButtonYellow;

    [SerializeField] private LeanTweenType buttonEase;
    [SerializeField] private LeanTweenType sine;

    private const float territoryMin = 0.2f;
    private const float territoryMax = 1.0f;
    private const float territoryDelta = territoryMax - territoryMin;
    private const float earthMax = 2.98f;
    private const float earthMin = -3.5f;
    private const float skyMax = -2.98f;
    private const float skyMin = 3.5f;
    private const float earthSkyDelta = earthMax - skyMax;
    private const float transistionPoint = 0.5f;

    private float fadeTime = 0.8f;

    private int horizontalClicks = 20;
    private int verticalClicks = 10;

    private CanvasGroup lavaMeterAlpha;
    private CanvasGroup forestMeterAlpha;

    [SerializeField] private float earthClickCounter = 1;
    [SerializeField] private float skyClickCounter = 1;
    [SerializeField] private float forestClickCounter = 1;
    [SerializeField] private float lavaClickCounter = 1;

    public UnityEvent GrowDecayCycle;
    private PushPullCycle pushPull;
    private ClickToPlay menuScript;

    public bool isTouching;
    public bool hasBowl; // Weather, and water can begin
    public bool earthAboveWater; // Forest & Lava should move
    
    private bool storyOne;
    private bool storyThree;
    private bool storySix;  
    private bool storySeven;
    private bool lavaCross;
    private bool forestCross;

    // Weather Enum
    public enum Weather { None, Mudslide, Wind, Hurricane, Thunder }
    int WeatherChance;
    Weather currentWeather;
    bool pullFlag = false;
    bool pushFlag = false;

    // Decay rates
    public float EarthDecayRate = 1f;
    public float SkyDecayRate = 1f;
    public float ForestDecayRate = 1f;
    public float LavaDecayRate = 1f;

    private void Awake()
    {
        menuScript = FindObjectOfType<ClickToPlay>();        
        pushPull = FindObjectOfType<PushPullCycle>();
    }

    private void Start()
    {
        // Default weather setting is none
        currentWeather = Weather.None;

        // Set each territory to it's minimum value
        lavaMeter.fillAmount = territoryMin;
        forestMeter.fillAmount = territoryMin;

        // Set buttons to initial size
        forest.localScale = new Vector3(0.5f, 0.5f, 0);
        lava.localScale = new Vector3(0.5f, 0.5f, 0);
        earthSpirit.localScale = new Vector3(0.5f, 0.5f, 0);
        skySpirit.localScale = new Vector3(0.5f, 0.5f, 0);

        // Disable lava & forest spirit buttons
        lava.LeanAlpha(0, 0);
        forest.LeanAlpha(0, 0);
        forestMeterAlpha = forestMeter.gameObject.GetComponent<CanvasGroup>();
        lavaMeterAlpha = lavaMeter.gameObject.GetComponent<CanvasGroup>();

        lavaMeterAlpha.alpha = 0;
        forestMeterAlpha.alpha = 0;

        closeButton.SetActive(false);
        closeButtonSprite = closeButton.GetComponent<Image>().sprite;

        // Hide the different water variations
        neutral.alpha = 1;
        hot.alpha = 0;
        leafy.alpha = 0;
        tea.alpha = 0;

        // If the menu script is diabled, disable the "click to play" text
        if (!menuScript.enabled)
            menuScript.playText.alpha = 0;
        else
            menuScript.playText.alpha = 1;
    }

    private void Update()
    {
        EarthSky();
        StoryEvents();

        if (earthAboveWater)
        {
            GrowAndDecay();
        }

        float forestScaler = ((0.5f * forestMeter.fillAmount) / territoryDelta) + 0.25f;
        float lavaScaler = ((0.5f * lavaMeter.fillAmount) / territoryDelta) + 0.25f;

        Vector3 forestButtonScale = new Vector3(forestScaler, forestScaler, 0);
        Vector3 lavaButtonScale = new Vector3(lavaScaler, lavaScaler, 0);

        if (isTouching)
        {           
            forest.LeanScale(forestButtonScale, 0.8f).setEase(LeanTweenType.easeInSine);
            lava.LeanScale(lavaButtonScale, 0.8f).setEase(LeanTweenType.easeInSine);
        }


        // David's secret key
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    theEarth.transform.position = new Vector3(0, , 0);
        //    theEarth.transform.position = new Vector3(0, 0, 0);
        //}

    }


    // Handles what happens when a spirit button is pressed
    public void SpiritButton(RectTransform spirit)
    {
        var scaleX = spirit.localScale.x;
        var scaleY = spirit.localScale.y;
        float scaleDiff = 0.5f / horizontalClicks;

        Vector3 newScale = new Vector3(scaleX + scaleDiff, scaleY + scaleDiff ,0);

        // Scale the button up a bit
        if (scaleX < 1 && scaleX > 0.09f)
            spirit.LeanScale(newScale, 0.8f).setEase(buttonEase);    
        
        // Increment the corresponding click counter
        if (spirit == forest && forestClickCounter < 10)
            forestClickCounter += 1;
        if (spirit == lava && lavaClickCounter < 10)
            lavaClickCounter += 1;
        if (spirit == earthSpirit && earthClickCounter < 10)
            earthClickCounter += 0.5f;
        if (spirit == skySpirit && skyClickCounter < 10)
            skyClickCounter += 0.5f;

        // Disable the pressed button
        spirit.gameObject.GetComponent<Button>().interactable = false;
    }


    // Called at the start of each push / pull cycle
    public void EnableButton()
    {
        //-------------------
        // ENABLE BUTTONS
        //-------------------

        if (earthAboveWater)
        {
            lavaMeterAlpha.LeanAlpha(1, fadeTime).setEase(sine);
            forestMeterAlpha.LeanAlpha(1, fadeTime).setEase(sine);
            lava.LeanAlpha(1, fadeTime).setEase(sine);
            forest.LeanAlpha(1, fadeTime).setEase(sine);
        }
        else
        {
            lavaMeterAlpha.LeanAlpha(0, fadeTime).setEase(sine);
            forestMeterAlpha.LeanAlpha(0, fadeTime).setEase(sine);
            lava.LeanAlpha(0, fadeTime).setEase(sine);
            forest.LeanAlpha(0, fadeTime).setEase(sine);
        }

        // If any of the spirit buttons are incavtive, enable them
        if (!forest.gameObject.GetComponent<Button>().interactable)
            forest.gameObject.GetComponent<Button>().interactable = true;      

        if (!lava.gameObject.GetComponent<Button>().interactable)
            lava.gameObject.GetComponent<Button>().interactable = true;
       
        if (!earthSpirit.gameObject.GetComponent<Button>().interactable)
            earthSpirit.gameObject.GetComponent<Button>().interactable = true;      

        if (!skySpirit.gameObject.GetComponent<Button>().interactable)
            skySpirit.gameObject.GetComponent<Button>().interactable = true;


        //-------------------
        // DECREMENT CLICK COUNTERS
        //-------------------

        if (forestClickCounter > 1)
            forestClickCounter -= 0.5f;

        if (lavaClickCounter > 1)
            lavaClickCounter -= 0.5f;

        if (earthClickCounter > 1)
            earthClickCounter -= 0.25f;

        if (skyClickCounter > 1)
            skyClickCounter -= 0.25f;

        if (LavaDecayRate > 1f)
            LavaDecayRate -= 0.25f;
        if (SkyDecayRate > 1f)
            SkyDecayRate -= 0.25f;
        if (EarthDecayRate > 1f)
            EarthDecayRate -= 0.25f;
        if (ForestDecayRate > 1f)
            ForestDecayRate -= 0.25f;


        //-------------------
        // WATER TRANSISTIONS
        //-------------------

        if (hasBowl)
        {
            // Check the lava teritory is past the water
            if (lavaMeter.fillAmount >= transistionPoint)
            {
                neutral.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
                leafy.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
                hot.LeanAlpha(1, 0.8f).setEase(LeanTweenType.easeInOutSine);
            }

            // Check the forest teritory is past the water
            if (forestMeter.fillAmount >= transistionPoint)
            {
                neutral.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
                hot.LeanAlpha(0, 0.8f).setEase(LeanTweenType.easeInOutSine);
                leafy.LeanAlpha(1, 0.8f).setEase(LeanTweenType.easeInOutSine);
            }
        }
    }


    // Moves the territories during their corresponding push / pull phase
    public void GrowAndDecay()
    {
        var forestX = forest.localScale.x;
        var lavaX = lava.localScale.x;
        var forestY = forest.localScale.y;
        var lavaY = lava.localScale.y;


        Vector3 smallForest = new Vector3(1 - lavaX, 1 - lavaY, 0);
        Vector3 smallLava = new Vector3(1 - forestX, 1 - forestY, 0);

        // Check for Wind
        if (currentWeather == Weather.Wind) 
        {           
            if (storyThree)
            {
                LavaDecayRate += 1f;
                WeatherEvent(currentWeather);
            }           
        }            

        // Check for Thunder
        if (currentWeather == Weather.Thunder) 
        {

            if (storyThree)
            {
                ForestDecayRate += 1f;
                WeatherEvent(currentWeather);
            }
        }

        //-------------------
        // PUSH CYCLE
        //-------------------

        // Check the Push bool from the Day / Night script
        if (pushPull.Push)
        {
            pullFlag = false;
            // Set Weather to none if Mudslide or Wind is not happening
            if (currentWeather != Weather.Hurricane || currentWeather != Weather.Thunder)
            {
                currentWeather = Weather.None;
                WeatherChance = Random.Range(1, 100);
            }

            // If no weather effect randomly set weather at 15% chance
            if (currentWeather == Weather.None)
            {
                // Check only once since this runs every frame
                // Sets weather to either hurricane or thunder
                if (WeatherChance <= 25 && !pushFlag)
                {
                    // Random Max value is not inclusive
                    currentWeather = (Weather)Random.Range((float)Weather.Hurricane, (float)Weather.Thunder + 1);
                    Debug.Log("Current Push weather: " + currentWeather);
                }
                pushFlag = true;
            }

            // Grow Lava
            if (lavaMeter.fillAmount < territoryMax)
                lavaMeter.fillAmount += ((territoryDelta / horizontalClicks) * lavaClickCounter) * Time.deltaTime;


            // Decay Forest
            if (forestMeter.fillAmount > territoryMin)
                forestMeter.fillAmount -= (territoryDelta / horizontalClicks) * ForestDecayRate * Time.deltaTime;



            // Check the bars are touching
            if (forestMeter.fillAmount + lavaMeter.fillAmount >= 1)
            {
                isTouching = true;

                // Equalise the progress bars
                forestMeter.fillAmount = 1 - lavaMeter.fillAmount;
            }
        }

        //-------------------
        // PULL CYCLE
        //-------------------

        else if (!pushPull.Push)
        {
            pushFlag = false;
            // Set Weather to none if Mudslide or Wind is not happening
            if (currentWeather != Weather.Mudslide || currentWeather != Weather.Wind)
            {
                //WeatherVal = 0;
                currentWeather = Weather.None;
                WeatherChance = Random.Range(1, 100);
            }

            // If no weather effect randomly set weather at 15% chance
            if (currentWeather == Weather.None)
            {
                // Check only once since this runs every frame
                // Sets weather to either mudslide or wind
                if (WeatherChance <= 25 && !pullFlag)
                {
                    // Random Max value is not inclusive
                    currentWeather = (Weather)Random.Range((float)Weather.Mudslide, (float)Weather.Wind + 1);
                    Debug.Log("Current Pull weather: " + currentWeather);
                }
                pullFlag = true;
            }

            // Grow Forest
            if (forestMeter.fillAmount < territoryMax)
                forestMeter.fillAmount += (territoryDelta / horizontalClicks * forestClickCounter) * Time.deltaTime;


            // Decay Lava
            if (lavaMeter.fillAmount > territoryMin)
                lavaMeter.fillAmount -= (territoryDelta / horizontalClicks) * LavaDecayRate * Time.deltaTime;


            // Check the bars are touching
            if (forestMeter.fillAmount + lavaMeter.fillAmount >= 1)
            {
                isTouching = true;

                // Equalise the progress bars
                lavaMeter.fillAmount = 1 - forestMeter.fillAmount;
            }
        }
    }

    public void EarthSky()
    {
        Vector3 landVector = theEarth.transform.position;
        Vector3 skyVector = theSky.transform.position;
        float step = 0.2f * Time.deltaTime;

        if (theEarth.transform.position.y >= theSky.transform.position.y + 0.1f)
        {            
            earthAboveWater = true;

            if (theEarth.transform.position.y <= theSky.transform.position.y + 1.2f)
            {
                hasBowl = true;
            }
            else
            {
                hasBowl = false;
            }
        }
        else
        {
            hasBowl = false;
            earthAboveWater = false;
        }

        // Check for Mudslide
        if (currentWeather == Weather.Mudslide) 
        {           
            if (storyThree)
            {
                EarthDecayRate += 1f;
                WeatherEvent(currentWeather);
            }
        }            

        // Check for Hurricane
        if (currentWeather == Weather.Hurricane) 
        {           
            if (storyThree)
            {
                SkyDecayRate += 1f;
                WeatherEvent(currentWeather);
            }
        }

        if (pushPull.Push)
        {
            // Grow Earth
            if (theEarth.transform.position.y < earthMax - (earthClickCounter / verticalClicks))
            {
                landVector = new Vector3(0, step, 0);
                theEarth.transform.position += (landVector / earthSkyDelta * verticalClicks) * earthClickCounter;
            }

            // Decay Sky
            if (theSky.transform.position.y < skyMin + (skyClickCounter / verticalClicks))
            {
                skyVector = new Vector3(0, step * SkyDecayRate, 0);
                theSky.transform.position += (skyVector / earthSkyDelta * verticalClicks);
            }
        }

        else if (!pushPull.Push)
        {
            // Grow Sky 
            if (theSky.transform.position.y > skyMax)
            {
                skyVector = new Vector3(0, step, 0);
                theSky.transform.position -= (skyVector / earthSkyDelta * verticalClicks) * skyClickCounter;
            }

            // Decay Earth
            if (theEarth.transform.position.y > earthMin + (earthClickCounter / verticalClicks))
            {
                landVector = new Vector3(0, step * EarthDecayRate, 0);
                theEarth.transform.position -= (landVector / earthSkyDelta * verticalClicks);
            }
        }

    }

    // Display weather pop-ups
    public void WeatherEvent(Weather weather)
    {
        weatherPopups.SetActive(true);
        closeButton.SetActive(true);
        weatherPopups.GetComponentInChildren<CanvasGroup>().alpha = 0;

        switch (weather)
        {
            //Push weathers
            case Weather.Hurricane:
                hurricanPopup.LeanAlpha(1, fadeTime).setEase(sine).setOnComplete(StopTime);
                closeButton.GetComponent<Image>().sprite = closeButtonYellow;
                break;
            case Weather.Thunder:
                thunderPopup.LeanAlpha(1, fadeTime).setEase(sine).setOnComplete(StopTime);
                closeButton.GetComponent<Image>().sprite = closeButtonBlue;
                break;
            // Pull Weathers
            case Weather.Wind:
                windPopup.LeanAlpha(1, fadeTime).setEase(sine).setOnComplete(StopTime);
                closeButton.GetComponent<Image>().sprite = closeButtonBlue;
                break;
            case Weather.Mudslide:
                mudPopup.LeanAlpha(1, fadeTime).setEase(sine).setOnComplete(StopTime);
                closeButton.GetComponent<Image>().sprite = closeButtonYellow;
                break;
        }
    }

    // Close the weather pop-up
    public void CloseMe()
    {
        closeButton.SetActive(false);
        weatherPopups.SetActive(false);

        // Restart game clock
        Time.timeScale = 1;
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public int StoryEvents()
    {
        //Intro
        if (!storyOne)
        {
            Debug.Log("1");
            storyOne = true;
            return 1;
        }
        // Earth & Sky - No Bowl
        else if (!hasBowl && earthAboveWater)
        {
            Debug.Log("2");
            storySeven = true;
            return 2;
        }
        // Earth & Sky - Has Bowl
        else if (hasBowl && !storyThree)
        {
            Debug.Log("3");
            storyThree = true;
            return 3;
        }
        // Lava crossing mid 
        else if (lavaMeter.fillAmount >= transistionPoint & !lavaCross)
        {
            lavaCross = true;
            Debug.Log("4");
            return 4;
        }
        // Forest crossing mid 
        else if (forestMeter.fillAmount >= transistionPoint && !forestCross)
        {
            forestCross = true;
            Debug.Log("5");
            return 5;
        }
        // Forest & Lava meet
        else if (isTouching && !storySix)
        {
            storySix = true;
            Debug.Log("6");
            return 6;
        }
        // Drowned Earth - No Bowl
        else if (storyThree && !earthAboveWater)
        {
            Debug.Log("7");
            return 7;
        }
        // Tea Party
        else if (isTouching && hasBowl) // TO DO: forestMeter.fillAmount >= 0.425f && forestMeter.fillAmount <= 0.575f
        {
            // TO DO: lavaMeter.fillAmount >= 0.425f && lavaMeter.fillAmount <= 0.575f
            
            // Start a timer as long as the conditions have been met

            // elpasedTime ++ 

            // if elapsedTime == 3.5 secs 

            // TEA!

            return 8;
        }
        else
            return 0;
    }
}
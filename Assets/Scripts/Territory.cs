using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private const float territoryMax = 0.88f;

    int clicksToPass = 10;

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

        if(spirit == treeSpiritRect)
        {
            if (greenProgressBar.fillAmount < 0.87f)
                greenProgressBar.fillAmount += 0.76f / clicksToPass;
        }
        else
        {
            if (redProgressBar.fillAmount < 0.87f)
                redProgressBar.fillAmount += 0.76f / clicksToPass;
        }
        
    }
}

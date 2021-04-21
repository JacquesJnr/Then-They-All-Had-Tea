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

    public void TreeSpiritPressed()
    {
        var scaleX = treeSpiritRect.localScale.x;
        var scaleY = treeSpiritRect.localScale.y;
        Vector3 newScale = new Vector3(scaleX + 0.05f, scaleY + 0.05f, 0);

        if (scaleX <= 1)
        {
            treeSpiritRect.LeanScale(newScale, 0.8f).setEase(buttonEase);
        }
        
        for(int i = 0; i > 100; i++)
        {
            
        }
        greenProgressBar.fillAmount = Mathf.Lerp(greenProgressBar.fillAmount, 1f, 0.01f);
    }

    public void LavaSpiritPressed()
    {
        var scaleX = lavaSpiritRect.localScale.x;
        var scaleY = lavaSpiritRect.localScale.y;
        Vector3 newScale = new Vector3(scaleX + 0.05f, scaleY + 0.05f, 0);
        
        if (scaleX <= 1)
        {
            lavaSpiritRect.LeanScale(newScale, 0.8f).setEase(buttonEase);
        }
    }
}

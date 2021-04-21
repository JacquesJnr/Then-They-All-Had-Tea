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

    public float greenMaximum = 0;
    public float redMaximum = 0;

    private const float territoryMin = 0.12f;


    private void Start()
    {
        // Set each territory to it's minimum value
        redProgressBar.fillAmount = territoryMin;
        greenProgressBar.fillAmount = territoryMin;
    }

    public void TreeSpiritPressed()
    {

    }

    public void LavaSpiritPressed()
    {

    }
}

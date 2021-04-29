using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUps : MonoBehaviour
{
    private Territory territory;

    private void Start()
    {
        territory = FindObjectOfType<Territory>();
    }

    private void Update()
    {
        switch (territory.StoryEvents()) 
        {
            case 1:
                break;
            default:
                break;
        }

    }
}

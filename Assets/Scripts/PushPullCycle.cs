using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushPullCycle : MonoBehaviour
{
    [SerializeField] public const float timer = 3.5f;
    [Range(0, timer)] public float elapsed = 0;
    public bool Push = true;

    private Territory territory;

    void Awake()
    {
        territory = FindObjectOfType<Territory>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= timer)
        {
            if (Push)
                Push = false;
            else
                Push = true;

            territory.GrowDecayCycle.Invoke();
            elapsed = 0;
        }
    }
}
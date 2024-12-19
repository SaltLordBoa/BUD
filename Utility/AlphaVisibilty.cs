using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlphaVisibilty : MonoBehaviour
{
    [Header("Controls")]
    public CanvasGroup fadeObject;
    public bool turnOff;

    [Header("Alpha")]
    public bool fadeIn = false;
    public bool fadeOut = false;
    public float progress = 0f;


    void Start()
    {
        fadeObject.alpha = 0;
    }

    private void Update()
    {
        if(fadeIn == true)
        {
            progress += Time.deltaTime;

            if(progress >= 1f)
            {
                progress = 1f;
                fadeIn = false;
            }
            fadeObject.alpha = Mathf.Lerp(0f, 1f, progress);
        }
        if(fadeOut == true)
        {
            progress -= Time.deltaTime;
            if(turnOff == true)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            else if (progress <= 0f)
            {
                progress = 0f;
                fadeOut = false;
            }
            fadeObject.alpha = Mathf.Lerp(0f, 1f, progress);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        fadeIn = true;
        fadeOut = false;
    }

    private void OnTriggerExit(Collider other)
    {
        fadeOut = true;
        fadeIn = false;
    }

    public void FadeIn()
    {
        fadeIn = true;
        fadeOut = false;
    }

    public void FadeOut()
    {
        fadeOut = true;
        fadeIn = false;
    }



    public IEnumerator AlphaOn()
    {
        progress += Time.deltaTime;
        float newAlpha = Mathf.Lerp(0, 1, progress);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, newAlpha);
        yield return null;
    }

}

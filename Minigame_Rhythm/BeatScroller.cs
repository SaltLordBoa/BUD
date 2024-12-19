using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatScroller : MonoBehaviour
{
    public RhythmGameManager rGM;
    public float beatTempo;
    public bool hasStarted;

    void Start()
    {
        beatTempo = beatTempo / 60;
        
    }

    public void StartGame()
    {
        StartCoroutine(CountdownToStart());
        rGM.StartGame();
    }

    
    void Update()
    {
        if(!hasStarted)
        {

        }
        else
        {
            transform.position += new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }

    public int countdownTime;
    public TextMeshProUGUI countdownDisplay;

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = "READY?";

            yield return new WaitForSeconds(1f);
            countdownDisplay.text = countdownTime.ToString();
            countdownTime--;
        }

        countdownDisplay.text = "START!";

        yield return new WaitForSeconds(1f);
        countdownDisplay.gameObject.SetActive(false);
        hasStarted = true;
    }
}

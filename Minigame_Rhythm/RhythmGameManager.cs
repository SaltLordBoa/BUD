using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class RhythmGameManager : MonoBehaviour
{

    public bool startPlaying;
    public BeatScroller beatScroller;
    public static RhythmGameManager instance;

    //progress bar?

    [Header("Audio")]
    public AudioSource endSong;
    public AudioSource mainSong;


    //player image animations/emote switching
    [Header("Player")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite mcBase;
    [SerializeField] Sprite mcMiss;
    [SerializeField] Sprite[] mcImages;
    
    int randomNumber;

    [Header("Effects")]
    public GameObject hitEffect;
    public GameObject missEffect;
    public Transform effectLocation;

    [Header("Scoring")]
    [SerializeField] private float totalNotes;
    [SerializeField] private float hitNotes;
    [SerializeField] private float missedNotes;
    public GameObject resultsPanel;
    public TextMeshProUGUI percentHitText, hitText, missText, rankText, finalScoreText;
    public int currentScore;
    public int scorePerNote = 5;
    public TextMeshProUGUI scoreText;

    public GameObject scoreF;
    public GameObject scoreDC;
    public GameObject scoreBA;
    public GameObject scoreS;

    [SerializeField] private float timeLength;
    private float timeRemaining;
    private bool songOver;

    void Start()
    {
        instance = this;
        scoreText.text = "Score: 0";

        totalNotes = FindObjectsOfType<NoteObject>().Length;
        timeRemaining = timeLength;
        DialogueManager.instance.GetComponentInChildren<StandardUIQuestTracker>().HideTracker();
    }

    public void StartGame()
    {
        startPlaying = true;
        mainSong.Play();
    }

    void Update()
    {


        if (startPlaying == true)
        {
            if (!mainSong.isPlaying && !resultsPanel.activeInHierarchy && songOver)
            {
                resultsPanel.SetActive(true);
                endSong.Play();


                hitText.text = "" + hitNotes;
                missText.text = "" + missedNotes;

                float totalHit = hitNotes;
                float percentHit = (totalHit / totalNotes) * 100f;

                percentHitText.text = percentHit.ToString("F1") + "%";

                string rankVal = "F";


                if (percentHit > 40)
                {
                    rankVal = "D";

                    if (percentHit > 55)
                    {
                        rankVal = "C";

                        if (percentHit > 70)
                        {
                            rankVal = "B";

                            if (percentHit > 85)
                            {
                                rankVal = "A";

                                if (percentHit > 95)
                                {
                                    rankVal = "S";
                                    scoreS.SetActive(true);
                                }
                            }
                        }
                    }
                }

                rankText.text = rankVal;

                finalScoreText.text = currentScore.ToString();

                if (percentHit < 40)
                {
                    scoreF.SetActive(true);
                }
                if (percentHit > 40 && percentHit < 70)
                {
                    scoreDC.SetActive(true);
                }
                if (percentHit > 70 && percentHit < 95)
                {
                    scoreBA.SetActive(true);
                }
            }
            timeRemaining -= Time.deltaTime;
        }
        
        if (timeRemaining <= 0)
        {
            songOver = true;
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit");

        currentScore += scorePerNote;
        scoreText.text = "Score: " + currentScore;
        var hFX = Instantiate(hitEffect, effectLocation.position, hitEffect.transform.rotation);
        Destroy(hFX, 0.5f);
        hitNotes++;
    }

    public void NoteMissed()
    {
        Debug.Log("Miss");
        spriteRenderer.sprite = mcMiss;
        var mFX = Instantiate(missEffect, effectLocation.position, missEffect.transform.rotation);
        Destroy(mFX, 0.5f);
        missedNotes++;
    }

    public void ChangeImage()
    {
        randomNumber = Random.Range(1, 4);
        spriteRenderer.sprite = mcImages[randomNumber - 1];
    }

    public void BaseImage()
    {
        spriteRenderer.sprite = mcBase;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetTracker()
    {
        DialogueManager.instance.GetComponentInChildren<StandardUIQuestTracker>().ShowTracker();
    }

}

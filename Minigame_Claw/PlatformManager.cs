using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlatformManager : MonoBehaviour
{
    public GameObject menu;
    public ClawControl claw;

    public float speed = 2f;
    private bool gameOver;

    [Header("Timer")]
    [SerializeField] private float timeLength;
    private float timeRemaining;
    public TextMeshProUGUI timerText;
    public int countdownTime;
    public TextMeshProUGUI countdownDisplay;


    [Header("Scoring")]
    [SerializeField] public float score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI gradeText;

    public GameObject scoreF;
    public GameObject scoreDC;
    public GameObject scoreBA;
    public GameObject scoreS;

    [Header("Bonus")]
    public List<GameObject> pickupList;
    public GameObject bonusPickup;
    public GameObject bonusText;

    private void Start()
    {
        timeRemaining = timeLength;
    }


    public void StartGame()
    {
        StartCoroutine(CountdownToStart());

    }

    public void Update()
    {
        scoreText.text = "Score: " + score;
        if(claw.canMove == true && gameOver == false)
        {
            timeRemaining -= Time.deltaTime;
        }
        timerText.text = $"Time: {(int)timeRemaining % 120:D3}";
        
        if (timeRemaining <= 0)
        {
            gameOver = true;
            GameOver();
        }

        //if finish early, activate bonus item that ends the game
        if(pickupList.Count == 0 && gameOver != true)
        {
            BonusItem();
        }
    }

    public void GameOver()
    {
        if (claw.moveComplete == true)
        {
            claw.canMove = false;
        }
        if (claw.dropped == true)
        {
            menu.SetActive(true);
        }
  
        finalScoreText.text = "Final Score: " + score;
        timeRemaining = 0;

        if(score < 10)
        {
            gradeText.text = "Grade: F";
            scoreF.SetActive(true);
        }
        if (score > 10 && score < 27)
        {
            if (score > 10 && score < 15)
            {
                gradeText.text = "Grade: D";
            }
            if (score > 15 && score < 27)
            {
                gradeText.text = "Grade: C";
            }
            scoreDC.SetActive(true);
        }

        if (score > 28 && score < 39)
        {
            if (score > 28 && score < 33)
            {
                gradeText.text = "Grade: B";
            }
            if (score > 33 && score < 39)
            {
                gradeText.text = "Grade: A";
            }
            scoreBA.SetActive(true);
        }
        if (score > 40)
        {
            gradeText.text = "Grade: S";
            scoreS.SetActive(true);
        }
    }


    public void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void IncreaseScore(int objectScoreAmount)
    {
        score += objectScoreAmount;
    }


    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSeconds(1f);
        countdownDisplay.gameObject.SetActive(false);
        claw.canMove = true;
    }



    void BonusItem()
    {
        bonusPickup.SetActive(true);
        bonusText.SetActive(true);
    }
}

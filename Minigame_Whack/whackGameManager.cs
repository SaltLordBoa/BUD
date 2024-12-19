using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class whackGameManager : MonoBehaviour
{
    [SerializeField] private List<Enemy_Spawn> wasps;

    [Header("UI objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject timeOver;
    [SerializeField] private GameObject deathText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI finalScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI gradeText;


    [SerializeField] private float startingTime = 30f;
    private float timeRemaining;
    private HashSet<Enemy_Spawn> currentSpawns = new HashSet<Enemy_Spawn>();
    private int score;
    private bool playing = false;

    [Header("Scoring")]
    [SerializeField] private GameObject scoreF;
    [SerializeField] private GameObject scoreDC;
    [SerializeField] private GameObject scoreBA;
    [SerializeField] private GameObject scoreS;

    [Header("Mouse")]
    public Texture2D cursorBase;
    public Texture2D cursorHit;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Start()
    {
        Cursor.SetCursor(cursorBase, hotSpot, cursorMode);
    }

    public void StartGame()
    {
        playButton.SetActive(false);
        timeOver.SetActive(false);
        deathText.SetActive(false);
        gameUI.SetActive(true);

        for (int i = 0; i < wasps.Count; i++)
        {
            wasps[i].Hide();
            wasps[i].SetIndex(i);
        }

        currentSpawns.Clear();

        timeRemaining = startingTime;
        score = 0;
        scoreText.text = "Wasp-Foxes eliminated:";
        playing = true;
    }


    public void GameOver(int type)
    {
        if (type == 0)
        {
            timeOver.SetActive(true);
            finalScoreText.text = $"Time: {(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";

            SetGrade();
        }
        else
        {
            deathText.SetActive(true);
        }

        foreach (Enemy_Spawn bug in wasps)
        {
            bug.StopGame();
        }

        playing = false;
        playButton.SetActive(true);
    }

    public void SetGrade()
    {
        if (timeRemaining > 120)
        {
            scoreF.SetActive(true);
            gradeText.text = "Grade: F";
        }


        if(timeRemaining < 120 && timeRemaining > 70)
        {
            if(timeRemaining > 90 && timeRemaining < 120)
            {
                gradeText.text = "Grade: D";
            }
            if(timeRemaining > 70 && timeRemaining < 90)
            {
                gradeText.text = "Grade: C";
            }

            scoreDC.SetActive(true);
        }

        if(timeRemaining > 30 && timeRemaining < 70)
        {
            if(timeRemaining > 45 && timeRemaining < 70)
            {
                gradeText.text = "Grade: B";
            }
            if(timeRemaining > 30 && timeRemaining < 45)
            {

                gradeText.text = "Grade: A";
            }

            scoreBA.SetActive(true);
        }
        
        if(timeRemaining < 30)
        {
            scoreS.SetActive(true);
            gradeText.text = "Grade: S";
        }

    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.SetCursor(cursorHit, hotSpot, cursorMode);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Cursor.SetCursor(cursorBase, hotSpot, cursorMode);
        }

        if (playing)
        {
            timeRemaining += Time.deltaTime;
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";

            if (currentSpawns.Count <= (score / 10))
            {
                int index = Random.Range(0, wasps.Count);
                if (!currentSpawns.Contains(wasps[index]))
                {
                    currentSpawns.Add(wasps[index]);
                    wasps[index].Activate(score / 10);
                }
            }

            if(score == 100)
            {
                GameOver(0);
            }

        }
    }

    public void AddScore(int bugIndex)
    {
        score += 1;
        scoreText.text = $"Wasp-Foxes eliminated: {score}";
        currentSpawns.Remove(wasps[bugIndex]);
    }

    public void Missed(int bugIndex, bool isBug)
    {
        currentSpawns.Remove(wasps[bugIndex]);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

}

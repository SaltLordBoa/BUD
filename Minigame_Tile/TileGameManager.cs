using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TileGameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private SpriteRenderer finalImage;
    [SerializeField] private float imageSpeed;    

    private List<Transform> tiles;
    private int emptyLocation;
    private int size;

    [Header("Timer")]
    [SerializeField] private float timeSpent;
    public TextMeshProUGUI timerText;
    private bool isTiming;

    [Header("Menus")]
    public GameObject startPanel;
    public GameObject endPanel;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gradeText;

    public GameObject scoreF;
    public GameObject scoreDC;
    public GameObject scoreBA;
    public GameObject scoreS;

    public void StartGame()
    {
        StartCoroutine(WaitShuffle(0.5f));
    }

    private void Start()
    {
        tiles = new List<Transform>();
        size = 3;
        CreateGameTiles(0.01f);
        timeSpent = 0;
    }

    private void Update()
    {
        if (isTiming == true && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                //go through the list, the index tells us the position
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i] == hit.transform)
                    {

                        //check each direction for valid move, break out on success, dont if fail
                        if (SwapIfValid(i, -size, size)) { break; }
                        if (SwapIfValid(i, +size, size)) { break; }
                        if (SwapIfValid(i, -1, 0)) { break; }
                        if (SwapIfValid(i, +1, size - 1)) { break; }
                    }
                }
            }
        }

        if (isTiming == true)
        {
            timeSpent += Time.deltaTime;
            timerText.text = $"Time Spent: {(int)timeSpent / 60}:{(int)timeSpent % 60:D2}";
            CheckCompletion();
        }

        if(gameFinish == true)
        {
            imageSpeed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0, 1, imageSpeed);
            finalImage.color = new Color(1, 1, 1, newAlpha);
        }
    }

    //Create the game board with size x size pieces
    private void CreateGameTiles(float gapThickness)
    {
        //width of each tile
        float width = 1 / (float)size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform tile = Instantiate(tilePrefab, gameTransform);
                tiles.Add(tile);
                //tiles will be in a game board going from -1 to +1
                tile.localPosition = new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0);
                tile.localScale = ((2 * width) - gapThickness) * Vector3.one;
                tile.name = $"{(row * size) + col}";
                //empty space on the bottom right at finish
                if ((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    tile.gameObject.SetActive(false);
                }
                else
                {
                    //map UV coords appropriately, 0->1
                    float gap = gapThickness / 2;
                    Mesh mesh = tile.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    //UV coord order: (0, 1), (1, 1), (0, 0), (1, 0)
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    //assign uvs to the mesh
                    mesh.uv = uv;
                }
            }
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i+offset) == emptyLocation))
        {
            //swap in game state
            (tiles[i], tiles[i + offset]) = (tiles[i + offset], tiles[i]);
            //swap transforms
            (tiles[i].localPosition, tiles[i + offset].localPosition) = ((tiles[i + offset].localPosition, tiles[i].localPosition));
            //update empty location
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        for ( int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].name != $"{i}")
            {
                return false;
            }
        }
        isTiming = false;
        StartCoroutine(FinishGame());
        return true;
    }

    private bool gameFinish = false;
    private IEnumerator FinishGame()
    {
        gameFinish = true;
        yield return new WaitForSeconds(3);
        GameFinish();
    }

    public void GameFinish()
    {
        gameFinish = false;
        endPanel.SetActive(true);
        timeText.text = $"Final Time: {(int)timeSpent / 60}:{(int)timeSpent % 60:D2}";
        
        if(timeSpent > 300)
        {
            gradeText.text = "Grade: F";
            scoreF.SetActive(true);
        }
        if(timeSpent < 300 && timeSpent > 120)
        {
            if(timeSpent < 300 && timeSpent > 180)
            {
                gradeText.text = "Grade: D";
            }
            if(timeSpent < 180 && timeSpent >= 120)
            {
                gradeText.text = "Grade: C";
            }
            scoreDC.SetActive(true);
        }
        if(timeSpent < 119 && timeSpent > 31)
        {
            if(timeSpent < 119 && timeSpent > 60)
            {
                gradeText.text = "Grade: B";
            }
            if(timeSpent < 60 && timeSpent > 30)
            {
                gradeText.text = "Grade: A";
            }
            scoreBA.SetActive(true);
        }
        if(timeSpent < 30)
        {
            gradeText.text = "Grade: S";
            scoreS.SetActive(true);
        }
    }

    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        isTiming = true;
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (size * size * size))
        {
            //pick a random location
            int rnd = Random.Range(0, size * size);
            //prevent undoing last move
            if(rnd == last) { continue; }
            last = emptyLocation;
            //try surrounding spaces looking for valid move
            if (SwapIfValid(rnd, -size, size))
            {
                count++;
            }
            else if(SwapIfValid(rnd, +size, size))
            {
                count++;
            }
            else if(SwapIfValid(rnd, -1, 0))
            {
                count++;
            }
            else if(SwapIfValid(rnd, +1, size -1))
            {
                count++;
            }

        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

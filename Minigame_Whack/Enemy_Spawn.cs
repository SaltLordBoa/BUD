using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawn : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private Sprite wasp;
    [SerializeField] private Sprite WaspHat;
    [SerializeField] private Sprite waspHatBroken;
    [SerializeField] private Sprite waspHit;
    [SerializeField] private Sprite waspHatHit;

    [Header("GameManager")]
    [SerializeField] private whackGameManager gameManager;

    private Vector2 startPosition = new Vector2(0f, -5.3f);
    private Vector2 endPosition = Vector2.zero;

    private float showDuration = 0.5f;
    private float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private Vector2 boxOffset;
    private Vector2 boxSize;
    private Vector2 boxOffsetHidden;
    private Vector2 boxSizeHidden;

    private bool hittable = true;
    public enum BugType { Wasp, Hat, Bunny};
    private BugType bugType;
    private float hatRate = 0.25f;
    private float bunnyRate = 0f;
    private int lives;
    private int bugIndex = 0;

    private IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;

        // Show enemy
        float elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
            
            boxCollider2D.offset = Vector2.Lerp(boxOffsetHidden, boxOffset, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSizeHidden, boxSize, elapsed / showDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        boxCollider2D.offset = boxOffset;
        boxCollider2D.size = boxSize;

        yield return new WaitForSeconds(duration);

        // Hide enemy
        elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
            
            boxCollider2D.offset = Vector2.Lerp(boxOffset, boxOffsetHidden, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSize, boxSizeHidden, elapsed / showDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = start;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;

        if(hittable)
        {
            hittable = false;
            gameManager.Missed(bugIndex, bugType != BugType.Bunny);
        }
    }

    public void Hide()
    {
        transform.localPosition = startPosition;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;
    }

    private IEnumerator QuickHide()
    {
        yield return new WaitForSeconds(0.25f);

        if(!hittable)
        {
            Hide();
        }
    }

    private void OnMouseDown()
    {
        if(hittable)
        {
            switch(bugType)
            {
                case BugType.Wasp:
                    spriteRenderer.sprite = waspHit;
                    gameManager.AddScore(bugIndex);
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());
                    hittable = false;
                    break;
                case BugType.Hat:
                    if(lives == 2)
                    {
                        spriteRenderer.sprite = waspHatBroken;
                        lives--;
                    }
                    else
                    {
                        spriteRenderer.sprite = waspHatHit;
                        gameManager.AddScore(bugIndex);
                        StopAllCoroutines();
                        StartCoroutine(QuickHide());
                        hittable = false;
                    }
                    break;
                case BugType.Bunny:
                    gameManager.GameOver(1);
                    break;
                default:
                    break;
            }
        }
    }

    private void CreateNext()
    {
        float random = Random.Range(0f, 1f);
        if(random < bunnyRate)
        {
            bugType = BugType.Bunny;
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
            random = Random.Range(0f, 1f);
            if(random < hatRate)
            {
                bugType = BugType.Hat;
                spriteRenderer.sprite = WaspHat;
                lives = 2;
            }
            else
            {
                bugType = BugType.Wasp;
                spriteRenderer.sprite = wasp;
                lives = 1;
            }
        }
        hittable = true;
    }

    private void SetLevel(int level)
    {
        bunnyRate = Mathf.Min(level * 0.01f, 0.2f);
        hatRate = Mathf.Min(level * 0.025f, 0.5f);

        //scaling
        float durationMin = Mathf.Clamp(1 - level * 0.01f, 0.001f, 1f);
        float durationMax = Mathf.Clamp(2 - level * 0.01f, 0.001f, 2f);
        duration = Random.Range(durationMin, durationMax);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        // Work out collider values.
        boxOffset = boxCollider2D.offset;
        boxSize = boxCollider2D.size;
        boxOffsetHidden = new Vector2(boxOffset.x, -startPosition.y / 2f);
        boxSizeHidden = new Vector2(boxSize.x, 0f);
    }

    public void Activate(int level)
    {
        SetLevel(level);
        CreateNext();
        StartCoroutine(ShowHide(startPosition, endPosition));
    }

    public void SetIndex(int index)
    {
        bugIndex = index;
    }

    public void StopGame()
    {
        hittable = false;
        StopAllCoroutines();
    }
}

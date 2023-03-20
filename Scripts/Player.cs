using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] GameObject pauseMenuCanvas;
    private bool GameIsPaused = false;

    [Space]

    [Header("Speed")]
    [SerializeField] float baseMoveSpeed = 5f;
    float currentMoveSpeed;

    public float CurrentMoveSpeed
    {
        get{return currentMoveSpeed;}
        set{currentMoveSpeed = value;}
    }

    [Space]

    [Header("Padding")]
    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;

    [Space]

    [Header("Sprites")]
    [SerializeField] Sprite[] playerSprites;    
    SpriteRenderer spriteRenderer;

    Vector2 rawInput;
    Vector2 minBounds;
    Vector2 maxBounds;

    void Awake()
    {
        currentMoveSpeed = baseMoveSpeed;
    }

    void Start()
    {
        InitBounds();    
    }

    public void SelectSprite(int classId)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerSprites[classId];

        UpdateCollider();
    }

    void UpdateCollider()
    {
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    void Update()
    {
        Move();
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0,0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1,1));
    }

    void Move()
    {
        Vector2 delta = rawInput * currentMoveSpeed * Time.deltaTime;

        // Player movement fits in main camera
        Vector2 newPos = new Vector2();
        newPos.x = Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPos.y = Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);

        transform.position = newPos;
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    void OnPause(InputValue value)
    {
        PauseOrContinue();
    }

    public void PauseOrContinue()
    {
        GameIsPaused = !GameIsPaused;
        pauseMenuCanvas.SetActive(GameIsPaused);
        Time.timeScale = GameIsPaused ? 0f : 1f;
        Cursor.visible = GameIsPaused;
    }
}

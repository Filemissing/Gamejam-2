using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static PlayerController player;

    public int score = 0;

    public int maxHealth = 2;
    public int health = 2;

    public int headSize = 1;

    public float playerSpeed = 5000;
    public float timeSpeed = 1.0f;

    public GameObject book;
    void Awake()
    {
        if (instance == null) instance = this;

        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        IEnumerator BooksLoop()
        {
            while (true)
            {
                GameObject newBook = Instantiate<GameObject>(book);
                yield return new WaitForSeconds(.3f);
            }
        }

        //StartCoroutine(BooksLoop());
    }

    [Header("UI")]
    public CanvasGroup endScreen;
    public CanvasGroup HUD;
    [HideInInspector] public bool gameEnded;
    public IEnumerator EndGame()
    {
        gameEnded = true;

        player.rb.linearVelocity = Vector3.zero;
        player.enabled = false;

        float headaExplodeSize = player.headCollider.transform.localScale.x + 1;
        while (player.headCollider.transform.localScale.x < headaExplodeSize)
        {
            player.headCollider.transform.localScale = player.headCollider.transform.localScale + Vector3.one * .05f;
            Debug.Log(player.headCollider.transform.localScale);
            Debug.Log(headaExplodeSize);
            yield return null;
        }
        Destroy(player.headCollider.gameObject);

        endScreen.alpha = 1.0f;
        endScreen.interactable = true;
        endScreen.blocksRaycasts = true;

        HUD.alpha = 0;

        while (Time.timeScale > 0.003f)
        {
            Time.timeScale -= .003f;
            Debug.Log(Time.timeScale);
            yield return null;
        }
        if(Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
    }

    void Update()
    {
        if (instance == null) instance = this;

        if (!gameEnded && health <= 0) StartCoroutine(EndGame());
        else if (gameEnded) return;

        player.speed = playerSpeed;
        Time.timeScale = timeSpeed;
    }
}

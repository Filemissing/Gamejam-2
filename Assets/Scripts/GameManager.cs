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

    public AudioClip headPopSound;
    void Awake()
    {
        if (instance == null) instance = this;

        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            yield return null;
        }
        Destroy(player.headCollider.gameObject);
        AudioSource.PlayClipAtPoint(headPopSound, player.headCollider.transform.position);

        endScreen.alpha = 1.0f;
        endScreen.interactable = true;
        endScreen.blocksRaycasts = true;

        HUD.alpha = 0;

        while (Time.timeScale > 0.003f)
        {
            Time.timeScale -= .003f;
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

        timeSpeed = Mathf.Lerp(timeSpeed, 1.0f, .5f);
    }
}

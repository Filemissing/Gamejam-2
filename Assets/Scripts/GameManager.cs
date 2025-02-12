using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static PlayerController player;
    private void Awake()
    {
        if (instance == null) instance = this;

        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public int score = 0;

    public int maxHealth = 2;
    public int health = 2;

    public int headSize = 1;

    public float playerSpeed = 5000;
    public float timeSpeed = 1.0f;

    public CanvasGroup endScreen;
    public void EndGame()
    {
        endScreen.alpha = 1.0f;
        endScreen.interactable = true;
        endScreen.blocksRaycasts = true;
        player.enabled = false;
    }

    void Update()
    {
        if(health <= 0) EndGame();

        player.speed = playerSpeed;
        Time.timeScale = timeSpeed;
    }
}

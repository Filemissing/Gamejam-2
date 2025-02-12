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

    public int health;

    public int headSize;

    public int headSize = 1;

    public float playerSpeed = 5000;
    public float timeSpeed = 1.0f;



    void Update()
    {
        if(health <= 0) EndGame();

        player.speed = playerSpeed;
        Time.timeScale = timeSpeed;
    }
}

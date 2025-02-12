using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static PlayerController player;

    public int score = 0;

    public int maxHealth = 2;
    public int health = 2;

    public int headSize = 1;

    public float playerSpeed;
    public float timeSpeed;

    public GameObject book;

    public CanvasGroup endScreen;
    public void EndGame()
    {
        endScreen.alpha = 1.0f;
        endScreen.interactable = true;
        endScreen.blocksRaycasts = true;
        player.enabled = false;
    }

    void SpawnWave()
    {
        
    }




    void Update()
    {
        if (instance == null) instance = this;

        if(player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //GameObject newBook = Instantiate<GameObject>(book);
    }
}

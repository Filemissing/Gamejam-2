using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score;

    public int health;

    public int headSize;

    public float playerSpeed;
    public float timeSpeed;

    public GameObject book;



    void SpawnWave()
    {
        
    }




    void Update()
    {
        if (instance == null) instance = this;

        GameObject newBook = Instantiate<GameObject>(book);
    }
}

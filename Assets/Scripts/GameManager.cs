using System.Collections;
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




    void Awake()
    {
        IEnumerator BooksLoop()
        {
            while (true)
            {
                GameObject newBook = Instantiate<GameObject>(book);
                yield return new WaitForSeconds(.3f);
            }
        }

        StartCoroutine(BooksLoop());
    }




    void Update()
    {
        if (instance == null) instance = this;
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float headSize;

    public float playerSpeed;
    public float timeSpeed;

    public GameObject book;




    void Update()
    {
        if (instance == null) instance = this;

        GameObject newBook = Instantiate<GameObject>(book);
    }
}

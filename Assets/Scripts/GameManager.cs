using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Statistics")]
    public int score;
    public int headSize;
    public float playerSpeed;
    public float timeSpeed;



    void Update()
    {
        if (instance == null) instance = this;
    }
}

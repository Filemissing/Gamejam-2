using UnityEngine;

public class Book : Projectile
{
    enum Reward
    {
        None,
        PlayerSpeed,
        TimeSpeed
    }

    void Collided()
    {
        Debug.Log("Collided");
    }
}

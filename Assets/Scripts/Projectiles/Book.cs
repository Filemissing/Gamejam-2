using UnityEngine;

public class Book : Projectile
{
    [Header("Book")]
    [SerializeField] Reward reward;

    enum Reward
    {
        None,
        PlayerSpeed,
        TimeSpeed
    }

    public override void Collided(Collider other)
    {
        if(alreadyCollided) return;
        base.Collided(other);
        switch (reward)
        {
            case Reward.None:
                GameManager.instance.headSize += 1;
                GameManager.instance.score += 100;
                break;

            case Reward.PlayerSpeed:
                GameManager.instance.playerSpeed += 1;
                GameManager.instance.headSize += 1;
                GameManager.instance.score += 200;
                break;

            case Reward.TimeSpeed:
                GameManager.instance.timeSpeed *= .95f;
                GameManager.instance.headSize += 1;
                GameManager.instance.score += 200;
                break;
        }
    }
}

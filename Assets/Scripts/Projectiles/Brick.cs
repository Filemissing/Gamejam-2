using UnityEngine;

public class Brick : Projectile
{
    [Header("Brick")]
    [SerializeField] int damage;

    public override void Collided(Collider other)
    {
        if(alreadyCollided) return;
        base.Collided(other);
        GameManager.instance.health -= damage;
    }
}

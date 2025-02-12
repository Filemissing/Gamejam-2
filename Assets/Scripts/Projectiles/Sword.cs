using UnityEngine;

public class Sword : Projectile
{
    [Header("Sword")]
    [SerializeField] int damage;

    public override void Collided(Collider other)
    {
        if (alreadyCollided) return;
        base.Collided(other);
        GameManager.instance.health -= damage;
    }
}

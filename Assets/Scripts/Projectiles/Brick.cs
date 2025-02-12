using UnityEngine;

public class Brick : Projectile
{
    [Header("Brick")]
    [SerializeField] int damage;

    public override void Collided()
    {
        base.Collided();
        GameManager.instance.health -= damage;
    }
}

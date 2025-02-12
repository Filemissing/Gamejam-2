using UnityEngine;

public class Brick : Projectile
{
    [SerializeField] int damage;

    public override void Collided()
    {
        if(alreadyCollided) return;
        base.Collided();
        GameManager.instance.health -= damage;
    }
}

using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] float scaleFactor = 0.1f;
    void Update()
    {
        transform.localScale = Vector3.one + Vector3.one * ((GameManager.instance.headSize - 1) * scaleFactor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {
            if (collision.transform.TryGetComponent<Projectile>(out Projectile projectile))
            {
                projectile.SendMessage("Collided");
            }
        }
    }
}

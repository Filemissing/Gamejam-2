using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] float scaleFactor = 0.1f;
    void Update()
    {
        if (GameManager.instance.gameEnded) return;

        transform.localScale = Vector3.one + Vector3.one * ((GameManager.instance.headSize - 1) * scaleFactor);
    }
}

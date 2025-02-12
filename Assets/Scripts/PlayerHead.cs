using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] float scaleFactor = 0.1f;
    void Update()
    {
        transform.localScale = Vector3.one + Vector3.one * ((GameManager.instance.headSize - 1) * scaleFactor);
    }
}

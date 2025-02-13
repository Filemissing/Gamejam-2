using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] float scaleFactor = 0.1f;
    public Transform hat;
    Vector3 hatStartPosition;
    private void Awake()
    {
        hatStartPosition = hat.position - transform.parent.position;
    }

    void Update()
    {
        hat.position = transform.parent.position + hatStartPosition + new Vector3(0, (transform.localScale.y - 1) / 2 * .46f, 0);

        if (GameManager.instance.gameEnded) return;

        transform.localScale = Vector3.one + Vector3.one * ((GameManager.instance.headSize - 1) * scaleFactor);
    }
}

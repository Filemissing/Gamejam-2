using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    protected PathType pathType;

    [Header("Movement")]
    [SerializeField] protected AnimationCurve positionCurve;
    [SerializeField] protected AnimationCurve speedCurve;
    [SerializeField] public Vector3 startPosition = new Vector3(0, 1, 0);
    [SerializeField] public Vector3 endPosition = new Vector3(0, 1, 5);

    [SerializeField] public float moveTime = 1;

    protected float startTime = 0;
    protected float endTime = 0;

    protected enum PathType
    {
        Parabole,
        Boomerang
    }


    void Move()
    {
        void MoveParabole()
        {
            Vector3 nextPosition = startPosition;
            float timePercentage = (Time.time - startTime) / (endTime - startTime);
            float positionPercentage = speedCurve.Evaluate(timePercentage);

            nextPosition.x += positionPercentage * (endPosition.x - startPosition.x);
            nextPosition.y += positionCurve.Evaluate(positionPercentage);
            nextPosition.z += positionPercentage * (endPosition.z - startPosition.z);

            transform.position = nextPosition;
        }

        switch (pathType) // Selecting correct movement method
        {
            case PathType.Parabole:
                MoveParabole();
                break;

            case PathType.Boomerang:
                Debug.Log("Boomerang");
                break;

            default:
                MoveParabole();
                break;
        }
    }


    void Awake()
    {
        startTime = Time.time;
        endTime = Time.time + moveTime;

        float randomCurveMuliplier = Random.Range(.80f, 1.20f);
        for (int i = 0; i < positionCurve.keys.Length; i++)
        {
            positionCurve.MoveKey(i, new Keyframe(positionCurve.keys[i].time, positionCurve.keys[i].value *= randomCurveMuliplier));
        }

        Move();
    }


    void FixedUpdate()
    {
        if (Time.time >= endTime) // Checks if not finished yet
        {
            Destroy(gameObject);
            return;
        }

        Move();
    }
}

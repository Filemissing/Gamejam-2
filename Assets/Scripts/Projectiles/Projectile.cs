using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Rigidbody rb;

    protected PathType pathType;

    [Header("Movement")]
    [Header("Parabole")]
    [SerializeField] float velocityMultiplier = 125;
    [SerializeField] float velocityUpMultiplier = 1.3f;

    [Header("Boomerang")]
    [SerializeField] protected AnimationCurve positionCurve;
    [SerializeField] protected AnimationCurve speedCurve;
    [SerializeField] public Vector3 startPosition = new Vector3(0, 1, 0);
    [SerializeField] public Vector3 endPosition = new Vector3(0, 1, -5);
    [SerializeField] public float moveTime = 1;

    protected float startTime = 0;
    protected float endTime = 0;
    protected float destroyTime;

    protected enum PathType
    {
        Parabole,
        Boomerang
    }


    void Move()
    {
        void MoveParabole()
        {
            if (rb.isKinematic) // Checks if first time moving
            {
                transform.position = startPosition;
                rb.isKinematic = false;

                Vector3 velocityVector = endPosition - startPosition;
                velocityVector.y = velocityUpMultiplier; //* Mathf.Abs((velocityVector.z + velocityVector.x) / 2);
                velocityVector *= velocityMultiplier;

                rb.AddForce(velocityVector);
            }


        }

        void MoveBoomerang()
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
                MoveBoomerang();
                break;

            default:
                MoveParabole();
                break;
        }
    }


    public virtual void Collided()
    {
        rb.isKinematic = true;
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        startTime = Time.time;
        endTime = Time.time + moveTime;

        switch (pathType) // Assign randomness
        {
            case (PathType.Parabole):
                break;

            case (PathType.Boomerang):
                float randomCurveMuliplier = Random.Range(.80f, 1.20f);
                for (int i = 0; i < positionCurve.keys.Length; i++)
                    positionCurve.MoveKey(i, new Keyframe(positionCurve.keys[i].time, positionCurve.keys[i].value *= randomCurveMuliplier));
                break;
        }

        Move();
    }


    void FixedUpdate()
    {
        if (Time.time >= endTime + destroyTime) // Checks if not finished yet
        {
            Destroy(gameObject);
            return;
        }

        Move();
    }
}

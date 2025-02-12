using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;

    [Header("Movement")]
    [SerializeField] protected PathType pathType;
    [SerializeField] public Vector3 startPosition = new Vector3(0, 1, 0);

    [Header("Parabole")]
    [SerializeField] float velocityMultiplier = 125;
    [SerializeField] float velocityUpMultiplier = 1.3f;
    [SerializeField] [Range(0, 5)] float angularVelocityMultiplier = 2f;

    [Header("Boomerang")]
    [SerializeField] protected AnimationCurve positionCurve;
    [SerializeField] protected AnimationCurve speedCurve;
    [SerializeField] public Vector3 endPosition = new Vector3(0, 1, -5);
    [SerializeField] public float moveTime = 4;

    protected float startTime = 0;
    protected float endTime = 0;
    protected float destroyTime = 4;

    protected enum PathType
    {
        Parabole,
        Boomerang,
        Sword
    }


    [Header("Sound")]
    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] float soundVolume = 1;


    void Move()
    {
        void MoveParabole()
        {
            if (rb.isKinematic) // Checks if first time moving
            {
                // Setting default values
                transform.position = startPosition;
                rb.isKinematic = false;


                // Do velocity
                Vector3 velocityVector = endPosition - startPosition;
                velocityVector.y = velocityUpMultiplier; //* Mathf.Abs((velocityVector.z + velocityVector.x) / 2);
                velocityVector *= velocityMultiplier;

                rb.AddForce(velocityVector);


                // Do angular velocity
                float angularMin = -5f;
                float angularMax = 5f;
                Vector3 angularVelocityVector = new Vector3(Random.Range(angularMin, angularMax), Random.Range(angularMin, angularMax), Random.Range(angularMin, angularMax));
                angularVelocityVector *= angularVelocityMultiplier;

                rb.AddTorque(angularVelocityVector);
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

        void MoveSword()
        {
            if (rb.isKinematic) // Checks if first time moving
            {
                // Setting default values
                transform.position = startPosition;
                rb.isKinematic = false;


                // Do velocity
                Vector3 velocityVector = endPosition - startPosition;
                velocityVector.y = velocityUpMultiplier; //* Mathf.Abs((velocityVector.z + velocityVector.x) / 2);
                velocityVector *= velocityMultiplier;

                rb.AddForce(velocityVector);


                // Do angular velocity
                Vector3 angularVelocityVector = new Vector3(40 * Random.Range(.8f, 1.2f), 0, 0);
                angularVelocityVector *= angularVelocityMultiplier;

                rb.AddTorque(angularVelocityVector);
            }
        }

        switch (pathType) // Selecting correct movement method
        {
            case PathType.Parabole:
                MoveParabole();
                break;

            case PathType.Boomerang:
                MoveBoomerang();
                break;

            case PathType.Sword:
                MoveSword();
                break;

            default:
                MoveParabole();
                break;
        }
    }

    protected bool alreadyCollided = false;
    protected float collisionDampening = 4f;
    public virtual void Collided(Collider other)
    {
        // Play random hit sound
        if (hitSounds.Length > 0)
        {
            audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
            audioSource.volume = soundVolume;
            audioSource.Play();
        }


        if(alreadyCollided) return;
        alreadyCollided = true;

        Vector3 collisionPoint = other.ClosestPoint(transform.position);

        Vector3 normal = (collisionPoint - other.transform.position).normalized;

        rb.linearVelocity = Vector3.Reflect(rb.linearVelocity / collisionDampening, normal);

        // Do angular velocity
        float angularMin = -5f;
        float angularMax = 5f;
        Vector3 angularVelocityVector = new Vector3(Random.Range(angularMin, angularMax), Random.Range(angularMin, angularMax), Random.Range(angularMin, angularMax));
        angularVelocityVector *= angularVelocityMultiplier;

        rb.AddTorque(angularVelocityVector);

        StartCoroutine(ShrinkLoop());
    }

    float shrinkSpeed = 1f;
    IEnumerator ShrinkLoop()
    {
        while (transform.localScale.x > 0)
        {
            transform.localScale -= Vector3.one * (shrinkSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

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
            
            case (PathType.Sword):
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

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    SphereCollider headCollider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        headCollider = GetComponentInChildren<SphereCollider>();
    }

    private void Update()
    {
        Move();

        Sleep();
    }

    [Header("Movement")]
    public bool canMove = true;
    public float speed;
    public float edgeDistance;
    void Move()
    {
        if (rb == null) throw new NullReferenceException("Rigidbody was not assigned correctly");

        float input = Input.GetAxis("Horizontal");

        float targetSpeed = input * speed * Time.deltaTime;

        float difference = targetSpeed - rb.linearVelocity.x;

        if (canMove) rb.AddForce(difference, 0, 0);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -edgeDistance, edgeDistance), transform.position.y, transform.position.z);
    }

    [Header("Sleep")]
    [SerializeField] KeyCode sleepKey = KeyCode.Space;
    [Tooltip("Time is in seconds")] public float sleepTime = 2f;
    public float defaultDamping = 0f;
    public float sleepDamping = 2f;
    void Sleep()
    {
        if(Input.GetKeyDown(sleepKey) && canMove)
        {
            StartCoroutine(SleepWait(sleepTime));
        }
    }

    IEnumerator SleepWait(float seconds)
    {
        canMove = false;
        rb.linearDamping = sleepDamping;
        yield return new WaitForSeconds(seconds);
        rb.linearDamping = defaultDamping;
        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Projectile"))
        {
            if (other.transform.TryGetComponent<Projectile>(out Projectile projectile))
            {
                projectile.SendMessage("Collided", headCollider);
            }
        }
    }
}

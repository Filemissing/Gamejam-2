using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public SphereCollider headCollider;
    [SerializeField] Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        headCollider = GetComponentInChildren<SphereCollider>();
        animator = GetComponent<Animator>();
    }

    void CheckIsSleeping()
    {
        if (!isSleeping) // Check if player is sleeping
            return;

        if (lastDownSizeTime <= Time.time - downSizeCooldown) // Checks if should decrease headSize
        {
            lastDownSizeTime = Time.time;
            GameManager.instance.headSize = (int)Mathf.Clamp(GameManager.instance.headSize - 1, 0, Mathf.Infinity);
        }
    }

    private void Update()
    {
        Move();
        CheckIsSleeping();
        Sleep();
    }

    [Header("Movement")]
    public bool canMove = true;
    public float speed;
    public float edgeDistance;
    bool isSleeping = false;
    float lastDownSizeTime = 0;
    float downSizeCooldown = .4f;

    void Move()
    {
        if (rb == null) throw new NullReferenceException("Rigidbody was not assigned correctly");

        float input = Input.GetAxis("Horizontal");
        //animator.SetInteger("MovementDirection", (int)Input.GetAxisRaw("Horizontal"));

        float targetSpeed = input * speed * Time.deltaTime;

        float difference = targetSpeed;// - rb.linearVelocity.x;

        if (canMove) rb.AddForce(difference, 0, 0);
        else rb.AddForce(rb.linearVelocity.x, 0, 0);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -edgeDistance, edgeDistance), transform.position.y, transform.position.z);




        if (input < 0)
            animator.Play("WalkLeft");
        else if (input > 0)
            animator.Play("WalkRight");
        else
            animator.Play("Idle");
    }

    [Header("Sleep")]
    [SerializeField] KeyCode sleepKey = KeyCode.Space;
    [Tooltip("Time is in seconds")] public float sleepStopDelay = 1f;
    public float defaultMass = 1f;
    public float sleepMass = 4f;
    void Sleep()
    {
        if(Input.GetKeyDown(sleepKey) && !isSleeping)
        {
            isSleeping = true;
            canMove = false;
            rb.mass = sleepMass;
            lastDownSizeTime = Time.time;
        }
        if (Input.GetKeyUp(sleepKey) && isSleeping)
        {
            IEnumerator StopSleeping()
            {
                yield return new WaitForSeconds(sleepStopDelay);
                isSleeping = false;
                canMove = true;
                rb.mass = defaultMass;
            }

            StartCoroutine(StopSleeping());
        }
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








    /*
        IEnumerator SleepWait(float seconds)
    {
        canMove = false;
        rb.linearDamping = sleepDamping;
        yield return new WaitForSeconds(seconds);
        rb.linearDamping = defaultDamping;
        canMove = true;
    }
    */
}

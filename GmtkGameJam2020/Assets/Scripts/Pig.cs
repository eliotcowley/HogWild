using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Pig : MonoBehaviour
{
    [HideInInspector]
    public bool IsBeingSuckedIntoBlackHole = false;

    [SerializeField]
    private float minTimeBeforeSwitchDirection = 1f;

    [SerializeField]
    private float maxTimeBeforeSwitchDirection = 10f;

    [SerializeField]
    private float wanderForceMin = 0.01f;

    [SerializeField]
    private float wanderForceMax = 0.1f;

    [SerializeField]
    private float panicDistance = 5f;

    [SerializeField]
    private float panicSpeed = 0.2f;

    [SerializeField]
    private float rotateSpeed = 5f;

    [SerializeField]
    private Animator animator;

    private AnimalState state = AnimalState.Wandering;
    private Rigidbody rb;
    private float wanderTimer = 0f;
    private float wanderTimeMax;
    private Vector3 wanderDir = Vector3.zero;
    private float wanderSpeed = 0f;
    private AudioSource audioSource;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.audioSource = GetComponent<AudioSource>();
        this.wanderTimeMax = Random.Range(this.minTimeBeforeSwitchDirection, this.maxTimeBeforeSwitchDirection);
        SetNewWanderForce();
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, Player.Instance.transform.position) < this.panicDistance)
        {
            if (this.state != AnimalState.Panicked)
            {
                this.state = AnimalState.Panicked;
                this.animator.SetBool(AnimationParameters.Panicked, true);
            }
        }
        else if (this.state != AnimalState.Wandering)
        {
            this.state = AnimalState.Wandering;
            this.animator.SetBool(AnimationParameters.Panicked, false);
        }

        switch (this.state)
        {
            case AnimalState.Wandering:
                if (this.wanderTimer < this.wanderTimeMax)
                {
                    this.wanderTimer += Time.deltaTime;
                }
                else
                {
                    SetNewWanderForce();
                }
                break;

            case AnimalState.Panicked:
                this.wanderDir = (this.transform.position - Player.Instance.transform.position).normalized;
                this.wanderDir.y = 0f;
                this.wanderSpeed = this.panicSpeed;
                break;
        }

        //this.transform.rotation = Quaternion.LookRotation(this.wanderDir);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.wanderDir), this.rotateSpeed * Time.deltaTime);
        this.transform.Translate(this.wanderDir * this.wanderSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PigPen))
        {
            GameManager.Instance.Score++;
            AudioManager.Instance.PlaySound(Sfx.PigCatch);
        }

        if (other.gameObject.CompareTag(Tags.BlackHole))
        {
            this.rb.detectCollisions = false;
            this.audioSource.Play();
            StartCoroutine(other.GetComponent<BlackHole>().ExplodeCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PigPen))
        {
            GameManager.Instance.Score--;
        }
    }

    private void SetNewWanderForce()
    {
        float x = Random.Range(-1, 1);
        float z = Random.Range(-1, 1);
        this.wanderDir = new Vector3(x, 0f, z);
        this.wanderSpeed = Random.Range(this.wanderForceMin, this.wanderForceMax);
        this.wanderTimer = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.panicDistance);
        Gizmos.DrawRay(this.transform.position, this.wanderDir);
    }
}

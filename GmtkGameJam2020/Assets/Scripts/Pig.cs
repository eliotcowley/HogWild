using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pig : MonoBehaviour
{
    [SerializeField]
    private float minTimeBeforeSwitchDirection = 1f;

    [SerializeField]
    private float maxTimeBeforeSwitchDirection = 10f;

    [SerializeField]
    private float wanderForceMin = 1f;

    [SerializeField]
    private float wanderForceMax = 10f;

    [SerializeField]
    private float panicDistance = 5f;

    [SerializeField]
    private float panicForce = 5f;

    private AnimalState state = AnimalState.Wandering;
    private Rigidbody rb;
    private float wanderTimer = 0f;
    private float wanderTimeMax;
    private Vector3 wanderForce = Vector3.zero;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
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
            }
        }
        else if (this.state != AnimalState.Wandering)
        {
            this.state = AnimalState.Wandering;
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
                this.wanderForce = (this.transform.position - Player.Instance.transform.position) * this.panicForce;
                break;
        }

        this.rb.AddForce(this.wanderForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PigPen))
        {
            GameManager.Instance.Score++;
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
        float x = Random.Range(-1, 1) * Random.Range(this.wanderForceMin, this.wanderForceMax);
        float z = Random.Range(-1, 1) * Random.Range(this.wanderForceMin, this.wanderForceMax);
        this.wanderForce = new Vector3(x, 0f, z);
        this.wanderTimer = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.panicDistance);
    }
}

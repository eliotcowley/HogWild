using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePig : MonoBehaviour
{
    [SerializeField]
    private float minTimeBeforeSwitchDirection = 1f;

    [SerializeField]
    private float maxTimeBeforeSwitchDirection = 10f;

    [SerializeField]
    private float wanderForceMin = 0.1f;

    [SerializeField]
    private float wanderForceMax = 0.5f;

    [SerializeField]
    private float rotateSpeed = 5f;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float timeBeforeAnimationMax = 0.1f;

    private float wanderTimer = 0f;
    private float wanderTimeMax;
    private Vector3 wanderDir = Vector3.zero;
    private float wanderSpeed = 0f;
    private int id = 0;
    private static int NumIds = 0;

    private void Start()
    {
        this.id = NumIds++;
        UnityEngine.Random.InitState(DateTime.Now.Millisecond + this.id);
        this.wanderTimeMax = UnityEngine.Random.Range(this.minTimeBeforeSwitchDirection, this.maxTimeBeforeSwitchDirection);
        SetNewWanderForce();
        float timeBeforeAnimation = UnityEngine.Random.Range(0f, this.timeBeforeAnimationMax);
        StartCoroutine(StartAnimatingCoroutine(timeBeforeAnimation));
    }

    private void Update()
    {
        if (this.wanderTimer < this.wanderTimeMax)
        {
            this.wanderTimer += Time.deltaTime;
        }
        else
        {
            SetNewWanderForce();
        }

        if (this.wanderDir.magnitude > 0f)
        {
            this.transform.rotation =
                Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.wanderDir), this.rotateSpeed * Time.deltaTime);
        }

        this.transform.Translate(this.wanderDir * this.wanderSpeed * Time.deltaTime, Space.World);
    }

    private void SetNewWanderForce()
    {
        float x = UnityEngine.Random.Range(-1, 1);
        float z = UnityEngine.Random.Range(-1, 1);
        this.wanderDir = new Vector3(x, 0f, z);
        this.wanderSpeed = UnityEngine.Random.Range(this.wanderForceMin, this.wanderForceMax);
        this.wanderTimer = 0f;
    }

    private IEnumerator StartAnimatingCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.animator.SetBool(AnimationParameters.Panicked, true);
    }
}

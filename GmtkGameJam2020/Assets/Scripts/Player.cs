using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    public bool CanMove = true;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float smoothTime = 0.5f;

    [SerializeField]
    private float rotateSpeed = 5f;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float inputThreshold = 0.1f;

    private CharacterController character;
    private Vector2 input = Vector2.zero;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.character = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 moveDir = new Vector3(this.input.x, 0f, this.input.y) * this.speed;
        Vector3 moveSmoothed = Vector3.SmoothDamp(this.velocity, moveDir, ref this.velocity, this.smoothTime);

        if ((moveSmoothed.magnitude > this.inputThreshold) && this.CanMove)
        {
            this.transform.rotation = 
                Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveSmoothed), this.rotateSpeed * Time.deltaTime);

            this.character.Move(moveSmoothed * Time.deltaTime);
            this.animator.SetFloat(AnimationParameters.Speed, Mathf.Abs(this.input.magnitude));
        }
        else
        {
            this.animator.SetFloat(AnimationParameters.Speed, 0f);
        }
    }

    public void OnMove(InputValue value)
    {
        this.input = value.Get<Vector2>();
    }
}

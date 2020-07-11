using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float smoothTime = 0.5f;

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
        this.character.Move(moveSmoothed * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        this.input = value.Get<Vector2>();
    }
}

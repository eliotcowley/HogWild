using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;
    public int Score 
    { 
        get => this.score;
        set
        {
            this.score = value;
            UIManager.Instance.SetScore(this.score);
        } 
    }

    [HideInInspector]
    public int TotalPigs = 0;

    [HideInInspector]
    public float Timer = 0f;

    [SerializeField]
    private Transform pigContainer;

    [SerializeField]
    private float TimeSeconds = 60f;

    [SerializeField]
    private float timeBetweenText = 1f;

    private bool timerOn = true;
    private bool canRestart = false;

    private void Awake()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        Instance = this;
        this.TotalPigs = this.pigContainer.childCount;
        this.Timer = this.TimeSeconds;
    }

    private void Update()
    {
        if (this.timerOn)
        {
            if (this.Timer >= 0f)
            {
                this.Timer -= Time.deltaTime;
            }
            else
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        this.timerOn = false;
        StartCoroutine(UIManager.Instance.SetGameOverCoroutine());
        this.canRestart = true;
        Player.Instance.CanMove = false;
    }

    public void OnSubmit(InputValue value)
    {
        if (this.canRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

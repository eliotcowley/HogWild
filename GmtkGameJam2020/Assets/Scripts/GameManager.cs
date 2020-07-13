using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float StartupTime = 3f;

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

    [HideInInspector]
    public bool IsTimerOn = false;

    [SerializeField]
    private Transform pigContainer;

    [SerializeField]
    private float TimeSeconds = 60f;

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
        if (this.IsTimerOn)
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
        this.IsTimerOn = false;
        StartCoroutine(UIManager.Instance.SetGameOverCoroutine());
        this.canRestart = true;
        Player.Instance.CanMove = false;
    }

    public void OnSubmit()
    {
        if (this.canRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    public void OnCancel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

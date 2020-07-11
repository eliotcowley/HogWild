using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        Instance = this;
    }
}

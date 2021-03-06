﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private TextMeshProUGUI timesUpText;

    [SerializeField]
    private TextMeshProUGUI restartText;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    private Vector3 shakeAmount = new Vector3(10f, 10f, 10f);

    [SerializeField]
    private float fadeTime = 1f;

    [SerializeField]
    private float shakeTime = 0.9f;

    [SerializeField]
    private float timeBetweenText = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetScore(0);
        StartCoroutine(StartCountdownCoroutine());
    }

    public void SetScore(int score)
    {
        this.scoreText.SetText($"{Strings.ScoreString} {score} / {GameManager.Instance.TotalPigs}");
    }

    public IEnumerator SetGameOverCoroutine()
    {
        LeanTween.value(this.timesUpText.gameObject, a => this.timesUpText.alpha = a, 0f, 1f, this.fadeTime);
        iTween.ShakePosition(this.timesUpText.gameObject, this.shakeAmount, this.shakeTime);
        iTween.ShakePosition(this.timerText.gameObject, this.shakeAmount, this.shakeTime);
        yield return new WaitForSeconds(this.timeBetweenText);
        LeanTween.value(this.restartText.gameObject, a => this.restartText.alpha = a, 0f, 1f, this.fadeTime);
    }

    private void Update()
    {
        int seconds = Mathf.CeilToInt(GameManager.Instance.Timer % 60);
        int minutes = (int)(GameManager.Instance.Timer / 60);
        this.timerText.SetText($"{minutes:D2} : {seconds:D2}");
    }

    private IEnumerator StartCountdownCoroutine()
    {
        int countdown = 3;
        ShowCountdownNumber(countdown--);
        yield return new WaitForSeconds(this.timeBetweenText);

        ShowCountdownNumber(countdown--);
        yield return new WaitForSeconds(this.timeBetweenText);

        ShowCountdownNumber(countdown--);
        yield return new WaitForSeconds(this.timeBetweenText);

        this.countdownText.SetText(Strings.Go);
        iTween.ShakePosition(this.countdownText.gameObject, this.shakeAmount, this.shakeTime);
        Player.Instance.CanMove = true;
        GameManager.Instance.IsTimerOn = true;
        yield return new WaitForSeconds(this.timeBetweenText);

        this.countdownText.SetText("");
    }

    private void ShowCountdownNumber(int number)
    {
        LeanTween.value(this.countdownText.gameObject, a => this.countdownText.alpha = a, 0f, 1f, this.fadeTime);
        iTween.ShakePosition(this.countdownText.gameObject, this.shakeAmount, this.shakeTime);
        this.countdownText.SetText(number.ToString());
    }
}

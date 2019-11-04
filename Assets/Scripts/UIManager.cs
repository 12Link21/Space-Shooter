﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _instructionsText;
    [SerializeField]
    private Text _controlsText;

    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;

    private Coroutine _instructionsTextCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
        UpdateLives(3);
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _controlsText.gameObject.SetActive(true);
        _instructionsTextCoroutine = StartCoroutine(FlickeringTextRoutine(_instructionsText));
        //StartCoroutine(_instructionsTextCoroutine);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The Game_Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives (int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void GameStartSequence()
    {
        StopCoroutine(_instructionsTextCoroutine);
        _instructionsText.gameObject.SetActive(false);
        _controlsText.gameObject.SetActive(false);
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        StartCoroutine(FlickeringTextRoutine(_gameOverText));
        _restartText.gameObject.SetActive(true);
    }

    private IEnumerator FlickeringTextRoutine(Text text)
    {
        while (true)
        {
            text.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }
}

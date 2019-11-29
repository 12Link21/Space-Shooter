using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text[] _scoreTexts;
    [SerializeField]
    private Image[] _livesImgs;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _instructionsText;
    [SerializeField]
    private Text _controlsText;

    [SerializeField]
    private GameObject _pauseMenuPanel;

    [SerializeField]
    private GameObject _mobilePausePanel;
    [SerializeField]
    private GameObject _mobileRestartButton;

    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;

    private Coroutine _instructionsTextCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        for (int c = 0; c < _scoreTexts.Length; c++)
        {
            UpdateScore(0, c);
        }
        for (int c = 0; c < _livesImgs.Length; c++)
        {
            UpdateLives(3, c);

        }
        _gameOverText.gameObject.SetActive(false);
        _bestScoreText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _pauseMenuPanel.SetActive(false);
        _mobilePausePanel.SetActive(false);
        _mobileRestartButton.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The Game_Manager is NULL");
        }
        
#if UNITY_ANDROID
        _instructionsText.transform.localPosition= new Vector3(0, -175, 0);
#else 
        if (_gameManager.IsSinglePlayer == true)
        {
            _controlsText.gameObject.SetActive(true);

        }
        else
        {
            _controlsText.gameObject.SetActive(false);
        }
        
#endif
        _instructionsTextCoroutine = StartCoroutine(FlickeringTextRoutine(_instructionsText));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score, int player)
    {
        _scoreTexts[player].text = "Score: " + score;
    }

    public void UpdateLives (int currentLives,int player)
    {
        if (currentLives > 0)
        {
            _livesImgs[player].sprite = _liveSprites[currentLives];
        } else if (currentLives <= 0)
        {
            _livesImgs[player].sprite = _liveSprites[0];
            //GameOverSequence();
        }
    }

    public void GameStartSequence()
    {
        StopCoroutine(_instructionsTextCoroutine);
        _instructionsText.gameObject.SetActive(false);
        _controlsText.gameObject.SetActive(false);
    }

    public void GameOverSequence(int game, int best)
    {
        //_gameManager.GameOver();
        StartCoroutine(FlickeringTextRoutine(_gameOverText));
        SetBestScoreText(game, best);
        _bestScoreText.gameObject.SetActive(true);
#if UNITY_ANDROID
        _mobileRestartButton.SetActive(true);
#else
        _restartText.gameObject.SetActive(true);
#endif
    }

    public void PauseMenuSequence(bool isPaused)
    {
#if UNITY_ANDROID
        _mobilePausePanel.SetActive(isPaused);
#else  
        _pauseMenuPanel.SetActive(isPaused);
#endif
    }

    public void SetBestScoreText (int game, int best)
    {
        string text = "Your Score: " + game + "\nBest Score: " + best;
        _bestScoreText.text = text;
    }

    private IEnumerator FlickeringTextRoutine(Text text)
    {
        while (true)
        {
            text.gameObject.SetActive(!text.gameObject.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }
}

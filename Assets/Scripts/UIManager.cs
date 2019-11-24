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
    private Text _restartText;
    [SerializeField]
    private Text _instructionsText;
    [SerializeField]
    private Text _controlsText;

    [SerializeField]
    private GameObject _pauseMenuPanel;

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
        for (int c =0; c < _livesImgs.Length; c++)
        {
            UpdateLives(3, c);

        }
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _pauseMenuPanel.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The Game_Manager is NULL");
        }

#if UNITY_STANDALONE_WIN
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

    public void GameOverSequence()
    {
        //_gameManager.GameOver();
        StartCoroutine(FlickeringTextRoutine(_gameOverText));
        _restartText.gameObject.SetActive(true);
    }

    public void PauseMenuSequence(bool isPaused)
    {
        _pauseMenuPanel.SetActive(isPaused);
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

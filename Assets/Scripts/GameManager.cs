using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    public bool IsGameOver
    {
        get { return _isGameOver; }
    }

    private bool _isGamePaused = false;

    public bool IsGamePaused
    {
        get { return _isGamePaused; }
    }

    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    private bool _isSinglePlayer = true;

    private List<GameObject> _players = new List<GameObject>();

    public bool IsSinglePlayer
    {
        get { return _isSinglePlayer; }
    }

    public List<GameObject> Players
    {
        get { return _players; }
    }

    private int _gameScore = 0;

    public int GameScore
    {
        get { return _gameScore; }
        set { _gameScore = value; }
    }

    private int _bestScore;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Single_Player")
        {
            _isSinglePlayer = true;
            _bestScore = PlayerPrefs.GetInt("HighScoreSingle", 0);
        }
        if (SceneManager.GetActiveScene().name == "Co-Op_Mode")
        {
            _isSinglePlayer = false;
            _bestScore = PlayerPrefs.GetInt("HighScoreCoop", 0);
        }

        GameObject[] playersArray = GameObject.FindGameObjectsWithTag("Player");

        if (playersArray.Length == 0)
        {
            Debug.LogError("No Player found");
        }

        foreach (GameObject item in playersArray)
        {
            _players.Add(item);
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager not found");
        }

    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_ANDROID
        if (CrossPlatformInputManager.GetButtonDown("Menu"))
        {
            GamePause();
        }
        if (CrossPlatformInputManager.GetButtonDown("Resume") && _isGamePaused == true)
        {
            GameResume();
        }
        if (CrossPlatformInputManager.GetButtonDown("MainMenu") && _isGamePaused == true)
        {
            ReturnMainMenu();
        }
        if (CrossPlatformInputManager.GetButtonDown("Exit") && _isGamePaused == true)
        {
            GameExit();
        }
        if (CrossPlatformInputManager.GetButtonDown("Restart") && _isGameOver == true)
        {
            RestartSinglePlayer();
        }
#else
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            if (_isSinglePlayer == true)
            {
                RestartSinglePlayer();
            }
            else
            {
                RestartCoOpMode();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if  (_isGamePaused == false)
            {
                GamePause();
            }
            else
            {
                GameResume();
            }
        }
#endif
    }

    public void GameStart()
    {
        _spawnManager.StartSpawning();
        _uiManager.GameStartSequence();
    }

    public void GameOver()
    {
        _isGameOver = true;
        _spawnManager.StopSpawning();
        if (_gameScore > _bestScore)
        {
            _bestScore = _gameScore;
            if (_isSinglePlayer == true)
            {
                PlayerPrefs.SetInt("HighScoreSingle", _bestScore);
            }
            else
            {
                PlayerPrefs.SetInt("HighScoreCoop", _bestScore);
            }
        }
        _uiManager.GameOverSequence(_gameScore, _bestScore);
    }

    public void GamePause()
    {
        _isGamePaused = true;
        Time.timeScale = 0;
        _uiManager.PauseMenuSequence(_isGamePaused);
    }

    public void GameResume()
    {
        _isGamePaused = false;
        Time.timeScale = 1;
        _uiManager.PauseMenuSequence(_isGamePaused);
    }

    public void GameExit()
    {
#if UNITY_EDITOR_WIN
        Debug.Log("Closing Application");
#endif
        Application.Quit();
    }

    public void RestartSinglePlayer()
    {
        SceneManager.LoadScene(1); // Loads Single_Player Scene
    }

    public void RestartCoOpMode()
    {
        SceneManager.LoadScene(2); // Loads Co-Op_Mode Scene
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0); // Loads Main_Menu Scene
    }
}

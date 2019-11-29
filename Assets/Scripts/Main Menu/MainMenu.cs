using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour
{
    /*[SerializeField]
    private GameObject _singlePlayerButton;
    [SerializeField]
    private GameObject _coOpModeButton;
    */
    [SerializeField]
    private GameObject _optionsPanel;
    [SerializeField]
    private GameObject _mobileOptionsPanel;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        _optionsPanel.SetActive(false);
        _mobileOptionsPanel.SetActive(true);
        //_singlePlayerButton.SetActive(false);
        //_coOpModeButton.SetActive(false);
#else
        _optionsPanel.SetActive(true);
        _mobileOptionsPanel.SetActive(false);
#endif
    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        if (CrossPlatformInputManager.GetButtonDown("SinglePlayer"))
        {
            SinglePlayerGame();
        }
        if (CrossPlatformInputManager.GetButtonDown("ExitGame"))
        {
            GameExit();
        }
#endif
    }

    public void SinglePlayerGame()
    {
        SceneManager.LoadScene(1); // Load Single_Player scene
    }

    public void CoOpModeGame()
    {
        SceneManager.LoadScene(2); // Load Co-Op_Mode scene
    }

    public void GameExit()
    {
#if UNITY_EDITOR_WIN
        Debug.Log("Closing Application");
#endif
        Application.Quit();
    }
}

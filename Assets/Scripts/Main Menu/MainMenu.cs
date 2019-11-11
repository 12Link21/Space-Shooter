using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _singlePlayerButton;
    [SerializeField]
    private GameObject _coOpModeButton;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        _singlePlayerButton.SetActive(false);
        _coOpModeButton.SetActive(false);
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
        if (CrossPlatformInputManager.GetButtonDown("CoOpMode"))
        {
            CoOpModeGame();
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
}

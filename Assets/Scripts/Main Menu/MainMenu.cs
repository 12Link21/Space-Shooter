using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _newGameButton;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        _newGameButton.SetActive(false);
#endif
    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        if (CrossPlatformInputManager.GetButtonDown("NewGame"))
        {
            LoadGame();
        }
#endif
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1); // Load Game scene
    }
}

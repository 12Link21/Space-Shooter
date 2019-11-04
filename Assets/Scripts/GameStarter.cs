using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
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

    public void SendStartMessage()
    {
        _gameManager.GameStart();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    private bool _mainMenu;

    public void Start()
    {
     
    }

    private void Update()
    {
        //if the r key was pressed
        //restart the current scene
        if (Input.GetKey(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); //Current Game Scene
        }
        if (Input.GetKeyDown(KeyCode.Escape) && _isGameOver == true)
        {
            SceneManager.LoadScene(0); //Main Menu
            _mainMenu = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && _mainMenu == true)
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}

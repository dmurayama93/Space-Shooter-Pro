using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //create a handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Image _ammoOne;
    [SerializeField]
    private Image _ammoTwo;
    [SerializeField]
    private Image _ammoThree;
    [SerializeField]
    private Image _thrusterFill;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _returnMainMenuText;
    [SerializeField]
    private Text _reloadText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _waveLevelText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _returnMainMenuText.gameObject.SetActive(false);
        _reloadText.gameObject.SetActive(false);
        _waveLevelText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is Null");
        }
    }
    public void WaveLevelText(int waveLevel)
    {
        _waveLevelText.text = "Wave " + waveLevel.ToString();
    }
    public void WaveLevelTrue()
    {
        _waveLevelText.gameObject.SetActive(true);
    }
    public void WaveLevelFalse()
    {
        _waveLevelText.gameObject.SetActive(false);
    }
    public void ReloadText()
    {
        _reloadText.gameObject.SetActive(true);
    }
    public void ReloadTextFalse()
    {
        _reloadText.gameObject.SetActive(false);
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }
    public void AmmoText(int ammoCount)
    {
        _ammoText.text = ammoCount.ToString() + "/15";
    }
    public void UpdateLives(int currentLives)
    {
        //is this how the uimanager knows that the game is over?
        
        if (currentLives > 0)
        {
            _livesImg.sprite = _liveSprites[currentLives];
        }
        if (currentLives == 0)
        {
            _livesImg.sprite = _liveSprites[currentLives];
            GameOverSequence();
        }
        if (currentLives < 0)
        {
            currentLives = 0;
            GameOverSequence();
        }
    }
    public void UpdateAmmoImage(bool _tripleActive)
    {
        if (_tripleActive == true)
        {
            _ammoOne.color = Color.green;
            _ammoTwo.color = Color.green;
            _ammoThree.color = Color.green;
        }
        if (_tripleActive == false)
        {
            _ammoOne.color = Color.white;
            _ammoTwo.color = Color.white;
            _ammoThree.color = Color.white;
        }
    }
    void GameOverSequence()
    {       
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _returnMainMenuText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "Game Over!!!";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}

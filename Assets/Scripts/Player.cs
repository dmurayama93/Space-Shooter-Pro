using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.Threading;
//using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2.0f;

    public float horizontalInput;
    public float verticalInput;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    private float _fireRate = 0.15f;
    private float _fireCD = -1f;
    
    [SerializeField]
    private int _playerLife = 3;
    private SpawnManager _spawnManager;

    private bool _tripleShotActive;
    private bool _speedBoostActive;

    [SerializeField]
    private bool _shieldActive;

    //variable reference to shield visualizer
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEnginePrefab;
    [SerializeField]
    private GameObject _leftEnginePrefab;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _playerExplosionSoundClip;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Doesnt Work");
        }

        if (_uiManager == null)
        {
            Debug.Log("UI Manager is null");
        }

        if (_audioSource == null)
        {
            Debug.Log("AudioSource is null on the player");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireCD)
        {
            FireLaser();
        }
    }   
   void CalculateMovement()
    {
        //Input in the quotations must match the axis in Unity > Edit > Project Settings > Axis
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //Thruster
        if (Input.GetKey("left shift"))
        {
            _speed = 10;
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            _speed = 5;
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        

        float _maxX = 9f;
        float _minX = -9f;
        float _maxY = 5.9f;
        float _minY = -4f;

        //X Max Boundary
        if(transform.position.x >= _maxX)
        {
            transform.position = new Vector3(_maxX, transform.position.y, 0);
        }

        //X Min Boundary
        if (transform.position.x <= _minX)
        {
            transform.position = new Vector3(_minX, transform.position.y, 0);
        }

        //Y Max Boundary
        if (transform.position.y >= _maxY)
        {
            transform.position = new Vector3(transform.position.x, _maxY, 0);
        }

        //Y Min Boundary
        if (transform.position.y <= _minY)
        {
            transform.position = new Vector3(transform.position.x, _minY, 0);
        }
        
    }
    
    void FireLaser()
    {
        _fireCD = Time.time + _fireRate;
        
        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        //play the laser audio clip
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_shieldActive == true)
        {
            _shieldVisualizer.SetActive(false);
            _shieldActive = false;
            return;
        }

        else
        {
            _playerLife = _playerLife - 1;
        }

        //if lives is 2 enable right engine
        //if lives is 1 enable left engine
        if (_playerLife == 2)
        {
            _rightEnginePrefab.SetActive(true);
        }
        else if (_playerLife == 1)
        {
            _leftEnginePrefab.SetActive(true);
        }
        else
        {
            _rightEnginePrefab.SetActive(false);
            _leftEnginePrefab.SetActive(false);
        }

        _uiManager.UpdateLives(_playerLife);
        
        if (_playerLife < 1)
        {
            //communicate with spawn manager when player is dead
            _spawnManager.OnPlayerDeath();
            _audioSource.clip = _playerExplosionSoundClip;
            _audioSource.Play();
            Destroy(this.gameObject, 2.5f);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostDownRoutine());
    }

    IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        _speedBoostActive = false;
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldDownRoutine());   
    }

    IEnumerator ShieldDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _shieldActive = false;
        _shieldVisualizer.SetActive(false);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Damage();
            Destroy(other.gameObject);
        }
    }
}
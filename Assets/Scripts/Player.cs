﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.Threading;
//using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
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
    private int _shieldStrength = 0;
    //SpriteRenderer _spriteRendererShield;

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

    [SerializeField]
    private GameObject Shield;
    private Shield _shield;



    //ammo
    [SerializeField]
    private int _actualAmmo;

    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shield = Shield.GetComponent<Shield>();

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

        _actualAmmo = 15;

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireCD && _tripleShotActive == true)
        {
            FireLaser();
        }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireCD && _actualAmmo > 0)
        {
            FireLaser();
            _actualAmmo--;
        }
        if (Input.GetKeyDown(KeyCode.Space) && _actualAmmo < 1)
        {
            Debug.Log("Reload");
            _uiManager.ReloadText();
        }
        if (_tripleShotActive == true)
        {
            _uiManager.ReloadTextFalse();
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
        if (_shieldActive == true && _shieldStrength > 0)
        {
            _shieldStrength--;
            _shield.ShieldStrength(_shieldStrength);
        }
        else
        {
            _playerLife--;
            _shieldActive = false;
        }

        if (_playerLife == 2)
        {
            _rightEnginePrefab.SetActive(true);
            _leftEnginePrefab.SetActive(false);
        }
        else if (_playerLife == 1)
        {
            _leftEnginePrefab.SetActive(true);
            _rightEnginePrefab.SetActive(true);
        }
        else if (_playerLife == 3)
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

    public void RefillAmmo()
    {
        _actualAmmo = 15;
        _uiManager.ReloadTextFalse();
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StopCoroutine(TripleShotPowerDownRoutine());
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
        StopCoroutine(SpeedBoostDownRoutine());
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
        _shieldStrength = 3;
        _shield.ShieldStrength(_shieldStrength);

        _shieldVisualizer.SetActive(true);

        StopCoroutine(ShieldDownRoutine());
        StartCoroutine(ShieldDownRoutine());
    }

    IEnumerator ShieldDownRoutine()
    {
        if (_shieldStrength < 1)
        {
            _shieldActive = false;
            _shieldVisualizer.SetActive(false);
        }

        yield return new WaitForSeconds(5.0f);
        _shieldActive = false;
        _shieldStrength = 0;
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

    public void PlusOneHP()
    {
        if (_playerLife == 3)
        {
            return;
        }
        else
        {
            _playerLife++;
            _uiManager.UpdateLives(_playerLife);
        }
        if (_playerLife == 2)
        {
            _rightEnginePrefab.SetActive(true);
            _leftEnginePrefab.SetActive(false);
        }
        else if (_playerLife == 1)
        {
            _leftEnginePrefab.SetActive(true);
            _rightEnginePrefab.SetActive(true);
        }
        else if (_playerLife == 3)
        {
            _rightEnginePrefab.SetActive(false);
            _leftEnginePrefab.SetActive(false);
        }
    }

    private void AmmoCount()
    {
        //max ammo is 15 shots
        //if actual ammo = 0, reload

    }
}
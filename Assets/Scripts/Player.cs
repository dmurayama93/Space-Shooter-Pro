using System.Collections;
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
    private GameObject _homingMissilePrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _ringPrefab;

    private float _fireRate = 0.15f;
    private float _fireCD = -1f;

    [SerializeField]
    private int _playerLife = 3;
    private SpawnManager _spawnManager;

    private bool _homingMissileActive;
    [SerializeField]
    private bool _tripleShotActive;
    [SerializeField]
    private bool _ringActive;
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

    public Thruster thruster;
    [SerializeField]
    private float _maxBoost = 100f;
    [SerializeField]
    private float _currentBoost;
    [SerializeField]
    private bool _thrusterDebuff;

    //ammo
    [SerializeField]
    private int _actualAmmo;

    public CameraShake cameraShake;

    public RingShot _ringShot;

    private GameObject _powerUp;

    [SerializeField]
    private GameObject _explosionPrefab;

    private bool _newTripleShot;

    private float _boostNeg = 1.5f;

    private bool _bossShake;

    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shield = Shield.GetComponent<Shield>();

        //set max boost at game start, and set in UI.
        _currentBoost = _maxBoost;
        thruster.SetMaxBoost(_currentBoost);

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
            _uiManager.AmmoText(_actualAmmo);
        }
        if (Input.GetKeyDown(KeyCode.Space) && _actualAmmo < 1)
        {
            //Debug.Log("Reload");
            _uiManager.ReloadText();
        }
        if (_tripleShotActive == true)
        {
            _uiManager.ReloadTextFalse();
        }
        if (_ringActive == true)
        {
            Instantiate(_ringPrefab, transform.position, Quaternion.identity);
            _ringActive = false;
        }

        if (_currentBoost >= 100)
        {
            _currentBoost = _maxBoost;
        }

        PickupCollect();
        HomingMissileFire();
        BossCameraShake();
    }
    void CalculateMovement()
    {
        //Input in the quotations must match the axis in Unity > Edit > Project Settings > Axis
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //Thruster
        if (Input.GetKey("left shift") && _currentBoost > 0 && _thrusterDebuff == false)
        {
            _speed = 10;
            _currentBoost -= _boostNeg;

            thruster.SetValue(_currentBoost);
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            _speed = 5;
            transform.Translate(direction * _speed * Time.deltaTime);
            if (_currentBoost <= 0)
            {
                //consider changing to a cd system versus coroutine
                StartCoroutine(ThrusterDelay());
            }
            else
            {
                _currentBoost += 0.5f;
                thruster.SetValue(_currentBoost);
            }
        }


        float _maxX = 9f;
        float _minX = -9f;
        float _maxY = 5.9f;
        float _minY = -4f;

        //X Max Boundary
        if (transform.position.x >= _maxX)
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

    IEnumerator ThrusterDelay()
    {
        yield return new WaitForSeconds(2.0f);
        _currentBoost += 0.5f;
        thruster.SetValue(_currentBoost);
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
            StartCoroutine(cameraShake.Shake(0.15f, 0.5f));
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

            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, .25f);
        }
    }

    public void RefillAmmo()
    {
        _actualAmmo = 15;
        _uiManager.ReloadTextFalse();
        _uiManager.AmmoText(_actualAmmo);
    }

    public void ThrusterDebuff()
    {
        _thrusterDebuff = true;
        //reference thruster script and call debuff method
        thruster.ThrusterDebuffActive(_thrusterDebuff);
        StartCoroutine(ThrusterDebuffDownRoutine());
    }
    IEnumerator ThrusterDebuffDownRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        _thrusterDebuff = false;
        thruster.ThrusterDebuffActive(_thrusterDebuff);
    }
    public void TripleShotActive()
    { 
        _tripleShotActive = true;
        _uiManager.UpdateAmmoImage(true);
        StopCoroutine("TripleShotPowerDownRoutine");
        StartCoroutine("TripleShotPowerDownRoutine");
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
        _uiManager.UpdateAmmoImage(false);

    }
    public void RingActive()
    {
        _ringActive = true;
    }
    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        if (_speedBoostActive == true)
        {
            _currentBoost = 100f;
            _boostNeg = 0f;
        }
        StopCoroutine("SpeedBoostDownRoutine");
        StartCoroutine("SpeedBoostDownRoutine");
    }

    IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        _speedBoostActive = false;
        _boostNeg = 1.5f;
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldStrength = 3;
        _shield.ShieldStrength(_shieldStrength);

        _shieldVisualizer.SetActive(true);

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
        if (other.tag == "BossBeam")
        {
            Damage();
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

    private void PickupCollect()
    {
        if (Input.GetKey(KeyCode.C))
        {
            _powerUp = GameObject.FindWithTag("PowerUp");
            if (_powerUp != null)
            {
                float _powerUpDist = Vector3.Distance(gameObject.transform.position, _powerUp.transform.position);

                if (_powerUpDist <= 7.0f)
                {
                    //Debug.Log("PowerUp Drag to Player " + _powerUpDist);
                    _powerUp.transform.position = Vector3.MoveTowards(_powerUp.transform.position, gameObject.transform.position, 5f * Time.deltaTime);
                }
            }
        }
    }
    public void HomingMissileActive()
    {
        _homingMissileActive = true;
    }
    public void HomingMissileFire()
    {
        if (Input.GetKeyDown(KeyCode.E) && _homingMissileActive == true)
        {
            Instantiate(_homingMissilePrefab, transform.position, Quaternion.identity);
            _homingMissileActive = false;
        }
    }
    public void BossDead(bool _bossDead)
    {
        if (_bossDead == true)
        {
            _bossShake = true;
        }
        if (_bossDead == false)
        {
            _bossShake = false;
        }
    }
    private void BossCameraShake()
    {
        if (_bossShake == true)
        {
            StartCoroutine(cameraShake.Shake(2.5f, .5f));
            _bossShake = false;
        }      
    }
}
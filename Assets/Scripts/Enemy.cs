using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 3f;
    [SerializeField]
    private float _speedDirection = 3f;

    private Player _player;

    //handle to animator component
    Animator _enemyAnimator;

    [SerializeField]
    private AudioClip _enemyExplosion;
    [SerializeField]
    private AudioClip _enemyLaserClip;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _enemyLaserPrefabUp;

    [SerializeField]
    private bool _enemyCD = true;

    //movement
    private int _movementRandom;
    private int _movementDirection;

    private bool _enemyShieldOn;

    private int _shieldRandom;
    private int _shieldStrength;
    [SerializeField]
    private GameObject _shieldVisualizer;

    private float boolTolerance = 1.0f;

    private bool _enemyDead;
    private bool _powerUpDead;

    private GameObject _powerUpObject;

    //public CameraShake camerashake;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.Log("No Player");
        }

        //assign component to anim
        _enemyAnimator = GetComponent<Animator>();

        if (_enemyAnimator == null)
        {
            Debug.Log("The Animator is Null");
        }

        if (_audioSource == null)
        {
            Debug.Log("Audio Source Null");
        }
        else
        {
            _audioSource.clip = _enemyLaserClip;
        }

        _powerUpObject = GameObject.FindWithTag("PowerUp");

        FireLaser();

        //Gets random variable for enemy movement at instantiation
        _movementRandom = Random.Range(0, 4);
        _movementDirection = Random.Range(0, 2);

        _shieldRandom = Random.Range(0, 4);

        if (_shieldRandom == 1)
        {
            _shieldStrength = 1;
        }
        if (_shieldRandom != 1)
        {
            _enemyShieldOn = false;
            _shieldVisualizer.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateEnemyMovement();
        SmartEnemy();
        KillPowerUp();
        AggressiveEnemy();
    }

    public void CalculateEnemyMovement()
    {
        if (_movementRandom == 3)
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
            MovementDirectionRoutine();

            if (_movementDirection == 0)
            {
                //Left
                transform.Translate(Vector3.left * _speedDirection * Time.deltaTime);
            }
            if (_movementDirection == 1)
            {
                //Right
                transform.Translate(Vector3.right * _speedDirection * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        }
        if (transform.position.y < -8f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8, 0);
            _movementRandom = Random.Range(0, 4);
        }
    }
    IEnumerator MovementDirectionRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        _movementDirection = Random.Range(0, 2);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            if (_shieldStrength == 1)
            {
                _shieldStrength--;
                _shieldVisualizer.SetActive(false);
                _enemyShieldOn = false;
            }
            else if (_shieldStrength == 0)
            {
                //trigger anim
                _enemyAnimator.SetTrigger("OnEnemyDeath");
                _enemySpeed = 0;
                _speedDirection = 0;

                //StartCoroutine(camerashake.Shake(0.15f, 0.4f));

                _audioSource.clip = _enemyExplosion;
                _audioSource.Play();

                _enemyDead = true;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }

        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            //if shield is active, else
            if (_shieldStrength == 1)
            {
                _shieldStrength--;
                _shieldVisualizer.SetActive(false);
                _enemyShieldOn = false;
                //return;
            }
            else if (_shieldStrength == 0)
            {
                _shieldVisualizer.SetActive(false);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                //trigger anim
                _enemyAnimator.SetTrigger("OnEnemyDeath");
                _enemySpeed = 0;
                _speedDirection = 0;

                _audioSource.clip = _enemyExplosion;
                _audioSource.Play();

                _enemyDead = true;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
        }

        if (other.tag == "Ring")
        {
            _shieldVisualizer.SetActive(false);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;

            _audioSource.clip = _enemyExplosion;
            _audioSource.Play();

            _enemyDead = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    private void FireLaser()
    {
        //add enemy fire every 3-7 seconds
        if (_enemyCD == true && _enemyDead == false)
        {
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.10f, 0), Quaternion.identity);
            _audioSource.clip = _enemyLaserClip;
            _audioSource.Play();

            _enemyCD = false;
            StartCoroutine(EnemyCDRoutine());
        }
    }
    private void FireLaserUp()
    {
        if (_enemyCD == true && _enemyDead == false)
        {
            Instantiate(_enemyLaserPrefabUp, transform.position + new Vector3(0, 0.10f, 0), Quaternion.identity);
            _audioSource.clip = _enemyLaserClip;
            _audioSource.Play();

            _enemyCD = false;
            StartCoroutine(EnemyCDRoutine());
        }
    }
    IEnumerator EnemyCDRoutine()
    {
        yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        _enemyCD = true;
    }
    private void EnemyShieldActive()
    {
        if (_shieldRandom == 1)
        {
            _enemyShieldOn = true;
            _shieldVisualizer.SetActive(true);
        }
    }
    private void SmartEnemy()
    {
        Vector3 localScale = transform.localScale;
        //if enemy y axis <= player y axis, rotate
        if (gameObject.transform.position.y < _player.transform.position.y && _player != null && _enemyDead == false)
        {
            localScale.y = -0.75f;
            transform.localScale = localScale;
            FireLaserUp();
        }
        if (gameObject.transform.position.y >= _player.transform.position.y && _player != null & _enemyDead == false)
        {
            localScale.y = 0.75f;
            transform.localScale = localScale;
            FireLaser();
        }
        else if (_player == null)
        {
            return;
        }
    }
    private void KillPowerUp()
    {
        float _enemyPosX = Mathf.Round(gameObject.transform.position.x * 100) / 100;
        
        if (_powerUpObject != null)
        {
            float _powerUpPosX = Mathf.Round(_powerUpObject.transform.position.x * 100) / 100;


            if (_enemyPosX <= _powerUpPosX + boolTolerance && _enemyPosX >= _powerUpPosX - boolTolerance)
            {              
                //Debug.Log("Shoot PowerUp " + _enemyPosX + " " + _powerUpPosX);
                FireLaser();
            }
        }
    }

    private void AggressiveEnemy()
    {
        float _distance = Vector3.Distance(gameObject.transform.position, _player.transform.position);

        if (_distance <= 5.0f)
        {
            _enemySpeed = 5.0f;
            //Debug.Log(_distance + " " + _enemySpeed);
        }
        else if (_distance > 5.0f)
        {
            _enemySpeed = 3.0f;
            //Debug.Log(_enemySpeed + " No Charge");
        }
        
    }
    /*private void DodgeBullet()
    { 

    }*/
}

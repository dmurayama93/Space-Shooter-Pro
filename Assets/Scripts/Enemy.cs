using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 4f;
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
    private bool _enemyCD = true;

    //movement
    private int _movementRandom;
    private int _movementDirection;

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

        FireLaser();

        //Gets random variable for enemy movement at instantiation
        _movementRandom = Random.Range(0, 4);
        _movementDirection = Random.Range(0, 2);

    }

    // Update is called once per frame
    void Update()
    {
        CalculateEnemyMovement();
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
            if(_movementDirection == 1)
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

            //Enables program to still run if the below scripts haven't been created yet
            if (player != null)
            {
                player.Damage();
            }
            //trigger anim
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _speedDirection = 0;

            //StartCoroutine(camerashake.Shake(0.15f, 0.4f));

            _audioSource.clip = _enemyExplosion;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            
           
        }

        if (other.tag == "Laser")
        {           
            Destroy(other.gameObject);

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

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            
        }

        if (other.tag == "Ring")
        {
            if(_player != null)
            {
                _player.AddScore(10);
            }

            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;

            _audioSource.clip = _enemyExplosion;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    private void FireLaser()
    {
        //add enemy fire every 3-7 seconds
        if (_enemyCD == true)
        {
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.10f, 0), Quaternion.identity);
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
}

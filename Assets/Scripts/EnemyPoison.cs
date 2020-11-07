using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoison : MonoBehaviour
{
    private Player _player;
    private PoisonCloud _poisonCloud;
    //public PoisonCloud poisonCloud;

    private int _movementRandom;
    private int _movementDirection;
    private float _enemySpeed = 2.0f;
    private float _speedDirection = 3.0f;
    private float _rotationSpeed = 120.0f;

    private bool _fireCD;
    private float _cdRandom;

    private bool _dead;

    [SerializeField]
    private GameObject _poisonCloudPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("No Player");
        }

        //Gets random variable for enemy movement at instantiation
        _movementRandom = Random.Range(0, 4);
        _movementDirection = Random.Range(0, 2);

        FirePoison();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FirePoison();
        //_poisonCloud.MovementParameters(_movementRandom, _movementDirection);
    }

    public void CalculateMovement()
    {
        transform.Rotate(0, 0, 1 * _rotationSpeed * Time.deltaTime);

        if (_movementRandom == 3)
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime, Space.World);
            //call movement direction coroutine
            StartCoroutine(MovementDirectionRoutine());

            if (_movementDirection == 0)
            {
                //Left
                transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime, Space.World);
            }
            if (_movementDirection == 1)
            {
                transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime, Space.World);
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
        _movementDirection = Random.Range(0, 4);
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

            //trigger anim
            _enemySpeed = 0;
            _speedDirection = 0;

            //audio

            //Destroy sequence
            _dead = true;
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
            _enemySpeed = 0;
            _speedDirection = 0;

            //audio

            //destroy sequence
            _dead = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Ring")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            //trigger anim
            _enemySpeed = 0;
            _speedDirection = 0;

            //audio

            //destroy sequence
            _dead = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    private void FirePoison()
    {
        // fire every 3 - 5 seconds
        //scale cloud from small to .1 - 1 over 1 sec
        if (_fireCD == false && _dead == false)
        {
            Instantiate(_poisonCloudPrefab, transform.position + new Vector3(-3.78f, -0.97f, 0f), Quaternion.identity);
            _fireCD = true;

            StartCoroutine(PoisonCD());
            //audio
        }

    }
    IEnumerator PoisonCD()
    {
        yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        _fireCD = false;
    }
}

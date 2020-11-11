using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    //movement, random
    [SerializeField]
    private int _randDirection;
    private float _speedDirection = 4.0f;
    private float _enemyChargeSpeed = 7.0f;

    private bool _chargeCD = true;
    private bool _changeDirection = true;
    //different attacks
    //standard laser, big beam, charge
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChargeCDRoutine());
        _randDirection = Random.Range(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (_changeDirection == true)
        {
            _changeDirection = false;
            StartCoroutine(MovementRoutine());
        }
    }

    private void CalculateMovement()
    {
        //StartCoroutine(MovementRoutine());
        if (_chargeCD == false)
        {
            _speedDirection = 0.0f;
            //charge forward, go down past bottom y and appear at top y same x
            transform.Translate(Vector3.down * _enemyChargeSpeed * Time.deltaTime);
            if (transform.position.y < -8f)
            {
                transform.position = new Vector3(transform.position.x, 5.08f, 0);
                _chargeCD = true;
                _speedDirection = 4.0f;
                StartCoroutine(ChargeCDRoutine());
            }
        }
        if (_randDirection == 1)
        {
            // move left
            transform.Translate(Vector3.left * _speedDirection * Time.deltaTime);
            Debug.Log(_randDirection);
        }
        if (_randDirection == 2)
        {
            //move right
            transform.Translate(Vector3.right * _speedDirection * Time.deltaTime);
            Debug.Log(_randDirection);
        }
        if (transform.position.x < -12f)
        {
            transform.position = new Vector3(12f, transform.position.y, 0);
        }
        if (transform.position.x > 12f)
        {
            transform.position = new Vector3(-12f, transform.position.y, 0);
        }
    }
    IEnumerator ChargeCDRoutine()
    {
        yield return new WaitForSeconds(Random.Range(4.5f, 8.0f));
        _chargeCD = false;
    }
    IEnumerator MovementRoutine()
    {       
        yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
        _randDirection = Random.Range(1, 3);
        _changeDirection = true;
    }
}

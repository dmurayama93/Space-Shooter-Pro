using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private bool _startChargeCD = true;

    //standard laser, big beam, charge
    //laser, ammo of 10 shots each side and then 3 secs reload
    [SerializeField]
    private GameObject _laserOnePrefab;
    [SerializeField]
    private GameObject _laserTwoPrefab;

    private bool _fireCD;
    private int _ammo;
    private int _ammoMax = 20;


    //hp make bar displaying hp as well
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChargeCDRoutine());
        _randDirection = Random.Range(1, 3);
        _ammo = _ammoMax;
        transform.position = new Vector3(0f, 8f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        InitialMovement();
        BossCharge();
        if (_changeDirection == true)
        {
            _changeDirection = false;
            StartCoroutine(MovementRoutine());
        }
        NormalLaser();
    }
    private void BossCharge()
    {
        if (_chargeCD == false)
        {
            _speedDirection = 0.0f;
            transform.Translate(Vector3.down * _enemyChargeSpeed * Time.deltaTime);
            if (_startChargeCD == true)
            {
                StartCoroutine(ChargeCDRoutine());
                _startChargeCD = false;
            }
                       
            if (transform.position.y < -8f)
            {
                transform.position = new Vector3(transform.position.x, 8f, 0);
                //initial movement may have to go here 
            }
        }
    }
    private void InitialMovement()
    {
        //set initial x pos to 0 in start, y pos to  8f
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (transform.position.y > 5.1f)
        {
            _speedDirection = 0.0f;
            transform.Translate(Vector3.down * 2f * Time.deltaTime);
            _chargeCD = true;
        }
        else if (transform.position.y <= 5.1f && _chargeCD == true)
        {
            _speedDirection = 4.0f;
            CalculateMovement();
        }

    }
    private void CalculateMovement()
    {
        if (_randDirection == 1)
        {
            // move left
            transform.Translate(Vector3.left * _speedDirection * Time.deltaTime);
        }
        if (_randDirection == 2)
        {
            //move right
            transform.Translate(Vector3.right * _speedDirection * Time.deltaTime);
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
        yield return new WaitForSeconds(Random.Range(8.0f, 12.0f));
        _chargeCD = false;
        _startChargeCD = true;

    }
    IEnumerator MovementRoutine()
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
        _randDirection = Random.Range(1, 3);
        _changeDirection = true;
    }

    private void NormalLaser()
    {
        //create fire rate cd
        if (_fireCD == true && _ammo > 0)
        {
            Instantiate(_laserOnePrefab, transform.position, Quaternion.identity);
            _ammo--;
            Debug.Log(_ammo + "Boss");
            Instantiate(_laserTwoPrefab, transform.position, Quaternion.identity);
            _ammo--;
            Debug.Log(_ammo + "Boss");
            StartCoroutine(NormalLaserFireRate());
        }
        if (_ammo <= 0)
        {
            _fireCD = false;
            StartCoroutine(NormalLaserReloadRoutine());
        }
    }
    IEnumerator NormalLaserReloadRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        _ammo = _ammoMax;
    }
    IEnumerator NormalLaserFireRate()
    {
        yield return new WaitForSeconds(0.25f);
        _fireCD = true;
    }

}

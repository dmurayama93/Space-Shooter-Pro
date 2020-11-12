using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    //movement attributes including charge
    [SerializeField]
    private int _randDirection;
    private float _speedDirection = 4.0f;
    private float _enemyChargeSpeed = 7.0f;

    private bool _chargeCD = true;
    private bool _changeDirection = true;
    private bool _startChargeCD = true;

    //boss prefabs
    [SerializeField]
    private GameObject _bossLaserPrefab;
    [SerializeField]
    private GameObject _bossBeamPrefab;

    //laser attributes
    private bool _fireCD = true;
    private int _ammo;
    private int _ammoMax = 10;
    private bool _reloading = true;

    //beam attributes
    private bool _bigBeamCD = true;
    private bool _reloadingBigBeam = true;
    private int _beamEnergy = 100;
    private bool _energyDown = true;

    private BossBeam _bossBeam;

    //hp
    private int _maxHealth = 5;
    private int _currHealth;
    private bool _bossDead = true;

    SpriteRenderer _spriteRendererBoss;
    private bool _damageFlash;

    private bool _stopMove;

    [SerializeField]
    private GameObject _explosionPrefab;
    //hit detection for player and boss

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChargeCDRoutine());
        _randDirection = Random.Range(1, 3);
        _ammo = _ammoMax;
        transform.position = new Vector3(0f, 8f, 0f);
        _currHealth = _maxHealth;
        _spriteRendererBoss = GetComponent<SpriteRenderer>();
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
        BigBeam();
        BossDestroy();
    }
    private void BossCharge()
    {
        if (_chargeCD == false && _stopMove == false)
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
        if (_stopMove == false)
        {
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
    }
    private void CalculateMovement()
    {
        if (_stopMove == false)
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
        else if (_stopMove == true)
        {
            _speedDirection = 0f;
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

    //Attacks
    private void NormalLaser()
    {
        //create fire rate cd
        if (_fireCD == true && _ammo > 0)
        {
            Instantiate(_bossLaserPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            _ammo--;
            //Debug.Log(_ammo + "Boss");
            _fireCD = false;
            if (_fireCD == false)
            {
                StartCoroutine(NormalLaserFireRate());
            }
        }
        if (_ammo <= 0)
        {
            _fireCD = false;
            if (_reloading == true)
            {
                StartCoroutine(NormalLaserReloadRoutine());
                _reloading = false;
            }          
        }
    }
    IEnumerator NormalLaserReloadRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        _ammo = _ammoMax;
        _fireCD = true;
        _reloading = true;
    }
    IEnumerator NormalLaserFireRate()
    {
        yield return new WaitForSeconds(1.5f);
        _fireCD = true;
    }
    private void BigBeam()
    {
        _bossBeam = _bossBeamPrefab.GetComponent<BossBeam>();
        if (_bigBeamCD == true && _beamEnergy > 0)
        {
            _bossBeamPrefab.SetActive(true);
            _bossBeam.BeamActive(true);
            if (_energyDown == true)
            {
                StartCoroutine(BigBeamEnergy());
                _energyDown = false;
            }
            Debug.Log(_beamEnergy);
        }
       
        if (_beamEnergy <= 0)
        {
            _bossBeamPrefab.SetActive(false);
            
            if (_reloadingBigBeam == true)
            {
                StartCoroutine(BigBeamCDRoutine());
                _bossBeam.BeamActive(false);
                _bossBeam.BeamResetScale();
                _reloadingBigBeam = false;
            }          
        }
    }
    IEnumerator BigBeamEnergy()
    {
        yield return new WaitForSeconds(0.25f);
        _beamEnergy -= 5;
        _energyDown = true;
    }
    IEnumerator BigBeamCDRoutine()
    {
        yield return new WaitForSeconds(15f);
        _bigBeamCD = true;
        _beamEnergy = 100;
        _reloadingBigBeam = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _currHealth--;
            Debug.Log(_currHealth + " " + other.tag);
            Destroy(other.gameObject);
            BossDamageFlash();
        }
        if (other.tag == "Player")
        {
            _currHealth--;
            Debug.Log(_currHealth + " " + other.tag);
            BossDamageFlash();
        }
        if (other.tag == "HomingMissile")
        {
            _currHealth -= 10;
            Debug.Log(_currHealth + " " + other.tag);
            Destroy(other.gameObject);
            BossDamageFlash();
        }
        if (other.tag == "RingShot")
        {
            _currHealth -= 20;
            Debug.Log(_currHealth + " " + other.tag);
            Destroy(other.gameObject);
            BossDamageFlash();
        }
    }

    private void BossDestroy()
    {
        if (_currHealth <= 0 && _bossDead == true)
        {
            _stopMove = true;

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 3f);

            _bossDead = false;
        }
    }
    private void BossDamageFlash()
    {
        _spriteRendererBoss.color = new Color(1, 1, 1, .5f);
        if (_damageFlash == false)
        {
            StartCoroutine(DamageFlashRoutine());
            _damageFlash = true;
        }
    }
    IEnumerator DamageFlashRoutine()
    {
        yield return new WaitForSeconds(.075f);
        _spriteRendererBoss.color = new Color(1, 1, 1, 1);
        _damageFlash = false;
    }
}

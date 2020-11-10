using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _circleEnemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _tripleShotPowerUpPrefab;
    [SerializeField]
    private GameObject _speedBoostPowerUpPrefab;
    [SerializeField]
    private GameObject _reloadPrefab;
    [SerializeField]
    private GameObject _hpOnePrefab;
    [SerializeField]
    private GameObject _ringPrefab;
    [SerializeField]
    private GameObject _thrustDebuffPrefab;
    [SerializeField]
    private GameObject _homingMissilePrefab;
    [SerializeField]
    private GameObject[] _powerUps;

    private float _circleEnemyCDStart;
    private float _circleEnemyCD;

    private float _enemyCD;
    

    private bool _stopSpawning = false;

    private float _powerUpSpawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        _circleEnemyCDStart = Random.Range(5.0f, 7.5f);
        _enemyCD = Random.Range(3.0f, 5.0f);
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(SpawnCircleEnemyRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity, _enemyContainer.transform);
            
            yield return new WaitForSeconds(_enemyCD);
        }
    }
    IEnumerator SpawnCircleEnemyRoutine()
    {
        yield return new WaitForSeconds(_circleEnemyCDStart);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newCircleEnemy = Instantiate(_circleEnemyPrefab, posToSpawn, Quaternion.identity, _enemyContainer.transform);
            _circleEnemyCD = Random.Range(4.0f, 12.0f);
            _circleEnemyCDStart = 0.0f;
            yield return new WaitForSeconds(_circleEnemyCD);
        }     
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        while (_stopSpawning == false)
        {
            _powerUpSpawnTimer = Random.Range(3.0f, 7.0f);
            int randomPowerUp = Random.Range(0, 14);

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_powerUpSpawnTimer);
        }
    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

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
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _tripleShotPowerUpPrefab;
    [SerializeField]
    private GameObject _speedBoostPowerUpPrefab;
    [SerializeField]
    private GameObject _reloadPrefab;
    [SerializeField]
    private GameObject[] _powerUps;
    

    private bool _stopSpawning = false;

    private float _powerUpSpawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
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
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        while (_stopSpawning == false)
        {
            _powerUpSpawnTimer = Random.Range(3.0f, 7.0f);
            int randomPowerUp = Random.Range(0, 4);

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

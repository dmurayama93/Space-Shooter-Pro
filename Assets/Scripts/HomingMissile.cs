using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    private GameObject _enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _enemy = FindClosestEnemy();

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (_enemy != null)
        {
                transform.position = Vector3.MoveTowards(gameObject.transform.position, _enemy.transform.position, 8.0f * Time.deltaTime);
        }

        FindClosestEnemy();
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }
}

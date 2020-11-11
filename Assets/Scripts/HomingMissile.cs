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
        //Set Speed
        //Define function
        //Reference enemy tags so missile knows who to lock on to.
        _enemy = GameObject.FindWithTag("Enemy");

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (_enemy != null)
        {
            float _homingDistance = Vector3.Distance(gameObject.transform.position, _enemy.transform.position);

            if (_homingDistance <= 8.0f)
            {
                transform.position = Vector3.MoveTowards(gameObject.transform.position, _enemy.transform.position, 8.0f * Time.deltaTime);
            }
        }
    }
}

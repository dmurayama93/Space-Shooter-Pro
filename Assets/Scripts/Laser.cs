using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speedLaser = 8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //translate laser up
        transform.Translate(Vector3.up * _speedLaser * Time.deltaTime);

        //when the laser passes y = 8 destroy clone
        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private float _speedLaser = 8f;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0, 0, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * _speedLaser * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}

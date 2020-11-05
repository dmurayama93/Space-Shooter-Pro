using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingShot : MonoBehaviour
{
    private float _shotTime;
    private float _maxTime;

    private bool _ringActive;

    //private GameObject _ringShot;

    Vector3 temp;

    SpriteRenderer _spriteRendererRing;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRendererRing = GetComponent<SpriteRenderer>();
        _maxTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shotTime < _maxTime)
        {
            _shotTime += Time.deltaTime;
            temp = transform.localScale;
            temp.x += Time.deltaTime * 15;
            temp.y += Time.deltaTime * 15;
            transform.localScale = temp;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}

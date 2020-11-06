using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    private float _shotTime;
    private float _maxSize;

    Vector3 temp;

    SpriteRenderer _spriteRenderePoison;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderePoison = GetComponent<SpriteRenderer>();
        _maxSize = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //when scale > 1 destroy
        if (temp.x < _maxSize)
        {
            _shotTime += Time.deltaTime;
            temp = transform.localScale;
            temp.x += Time.deltaTime * 5f;
            temp.y += Time.deltaTime * 5f;
            transform.localScale = temp;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

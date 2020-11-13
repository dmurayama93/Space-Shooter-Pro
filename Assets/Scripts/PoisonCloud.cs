using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    private float _shotTime;
    private float _maxSize;

    private float _maxTime;
    private float _time;

    private float _rotationSpeed = 120.0f;
    private float _enemySpeed = 3.0f;

    private GameObject EnemyPoison;
    private EnemyPoison enemyPoison;

    private int _movementDirection;
    private int _movementRandom;

    private Player _player;

    Vector3 temp;

    SpriteRenderer _spriteRenderePoison;
    // Start is called before the first frame update
    void Start()
    {
        //enemyPoison = EnemyPoison.GetComponent<EnemyPoison>();
        _spriteRenderePoison = GetComponent<SpriteRenderer>();
        _maxSize = 0.4f;

        if (_player != null)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }
        
        if (_player == null)
        {
            Debug.Log("No Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.time;
        //when scale > 1 destroy
        if (temp.x < _maxSize)
        {
            _shotTime += Time.deltaTime;
            temp = transform.localScale;
            temp.x += Time.deltaTime * .3f;
            temp.y += Time.deltaTime * .3f;
            transform.localScale = temp;
        }
       if (temp.x > _maxSize)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player _player = other.gameObject.transform.GetComponent<Player>();

        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }

            Destroy(GetComponent<Collider2D>());
        }
    }
}

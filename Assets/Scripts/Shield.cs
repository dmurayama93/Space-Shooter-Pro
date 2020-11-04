using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //3 Hit Shield
    //Visualize by changing colors white > yellow > red
    //[SerializeField]
    //private int _shieldLives;

    SpriteRenderer _spriteRendererShield;

    private Player _player;

    private bool _notWhite;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRendererShield = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShieldStrength(int _shieldLives)
    {

        //_player.ShieldActive();

        if (_shieldLives == 3 && _notWhite == true)
        {
            _spriteRendererShield.color = Color.white;
            _spriteRendererShield.enabled = true;
            return;
        }

        if (_shieldLives == 2)
        {
            _spriteRendererShield.color = Color.yellow;
            _notWhite = true;
        }
        if (_shieldLives == 1)
        {
            _spriteRendererShield.color = Color.red;
            _notWhite = true;
        }
        if (_shieldLives == 0)
        {
            _notWhite = true;
            _spriteRendererShield.enabled = false;
            return;
        }

    }
}

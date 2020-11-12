using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    SpriteRenderer _spriteRendererBeam;
    //scale laser so it doesnt just pop up
    //fix position
    // y 2.7f

    // Start is called before the first frame update
    void Start()
    {
        _spriteRendererBeam = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeamActive(bool _beamActive)
    {
        if (_beamActive == true)
        {
            _spriteRendererBeam.enabled = true;
        }
        if (_beamActive == false)
        {
            _spriteRendererBeam.enabled = false;
        }
    }
}

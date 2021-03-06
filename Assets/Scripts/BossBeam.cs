﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    private float _maxY = 2.5f;
    Vector3 temp;
    [SerializeField]
    private bool _beamActive;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _bossBeam;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_bossBeam);
    }
    // Update is called once per frame
    void Update()
    {
        BeamScaleUp();
        BeamResetScale();
    }
    private void BeamScaleUp()
    {
        if (_beamActive == true)
        {
            //Debug.Log("Sequence true");
            if (temp.y <= _maxY)
            {
                temp = transform.localScale;
                temp.y += Time.deltaTime * 1f;
                transform.localScale = temp;
            }
            if (temp.y > _maxY)
            {
                temp.y = _maxY;
                transform.localScale = temp;
            }
        }
    }
    public void BeamResetScale()
    {
        if (_beamActive == false)
        {
            temp.y = .1f;
            transform.localScale = temp;
            //Debug.Log("Resetting Scale " + temp.y);
        }
    }
    //create method to be called in boss script that sets temp back to default when disabled
    public void BeamActive(bool _active)
    {
        if (_active == true)
        {
            _beamActive = true;
        }
        if (_active == false)
        {
            _beamActive = false;
        }
    }
}

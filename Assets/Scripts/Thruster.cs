using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thruster : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public Player player;

    private bool _debuff;
    public void SetMaxBoost(float boost)
    {
        //sets boost bar to max at start of the game
        slider.maxValue = boost;
        slider.value = boost;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(float boost)
    {
        slider.value = boost;
    }

    public void ThrusterDebuffActive(bool _debuff)
    {
        if (_debuff == true)
        {
            fill.color = new Color(193, 0, 25, 255);
        }
        else
        {
            fill.color = gradient.Evaluate(1f);
        }
        
    }
}

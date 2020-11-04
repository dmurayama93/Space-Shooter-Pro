using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thruster : MonoBehaviour
{
    public Slider slider;
   
    public void SetMaxBoost(float boost)
    {
        //sets boost bar to max at start of the game
        slider.maxValue = boost;
        slider.value = boost;
    }

    public void SetValue(float boost)
    {
        slider.value = boost;
    }
}

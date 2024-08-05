using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fps : MonoBehaviour
{
    public TMP_Text fpssText;

    [SerializeField]
    private float _refreshTime = .5f;
    private int _frameCounter;
    private float _timeCounter;
    private float _fps;


    void Update()
    {
        if (_timeCounter < _refreshTime)
        {
            _timeCounter += Time.deltaTime;
            _frameCounter++;

        }
        else
        {
            _fps = _frameCounter / _timeCounter;
            _frameCounter = 0;
            _timeCounter = 0;
        }
        fpssText.text = "Fps: " + _fps.ToString();

    }
}
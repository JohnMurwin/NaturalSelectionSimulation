using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private int _secondsCounter;
    private float _timer;
    public float _delayAmount;

    // Start is called before the first frame update
    void Start()
    {
        _secondsCounter = 0;
        _timer = 0f;
        _delayAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _delayAmount)
        {
            _timer = 0f;
            _secondsCounter++;
            Debug.Log(_secondsCounter);
        }
    }
}

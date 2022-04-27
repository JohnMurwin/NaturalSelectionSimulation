using NaturalSelectionSimulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    /*
    private Animator _animator;
    private float _lifeExpectancyLimit;
    private float _lifeTimeCounter = 0f;
    private float _timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _lifeExpectancyLimit = gameObject.GetComponent<RabbitGeneCollection>().LifeExpectancy;
    }

    // Update is called once per frame
    void Update()
    {
        _timePassed += Time.deltaTime;
        if(_timePassed > 1f)
        {
            _lifeTimeCounter++;
            if(_lifeTimeCounter >= _lifeExpectancyLimit)
            {
                ExecuteDeath();
            }
            _timePassed = 0f;
        }
    }

    void ExecuteDeath()
    {
        ExecuteDeathAnimation();
        Destroy(gameObject, 5f);       
    }

    private void ExecuteDeathAnimation()
    {
        _animator.SetBool("isDead_0", true);
    }
    */
}

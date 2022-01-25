using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int Counter { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Counter++;
    }
}

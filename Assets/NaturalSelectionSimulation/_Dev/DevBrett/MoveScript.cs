using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{

    [SerializeField] private Transform[] Positions;
    [SerializeField] private float speed;

    int nextPosIndex;
    Transform nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = Positions[0];
    }

    // Update is called once per frame
    void Update()
    {
        moveObject();
    }

    void moveObject(){
        if(transform.position == nextPosition.position){
            nextPosIndex++;
            if(nextPosIndex >= Positions.Length){
                nextPosIndex = 0;
            }
            nextPosition = Positions[nextPosIndex];

        } else{
            transform.position = Vector3.MoveTowards(transform.position, nextPosition.position, (speed * Time.deltaTime));
        }
    }
    //cannot convert from 'UnityEngine.Transform' to 'UnityEngine.Vector3
}

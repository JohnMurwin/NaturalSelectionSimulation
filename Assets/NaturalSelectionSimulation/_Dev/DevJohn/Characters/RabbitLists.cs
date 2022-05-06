using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class RabbitLists : MonoBehaviour
    {
        public GameObject[] rabbits;

        private void Update() 
        {
            //TODO: REMOVE THIS FROM UPDATE EVENTUALLY (TIE INTO SIMULATION TICK LATER)
            rabbits = GameObject.FindGameObjectsWithTag("Rabbit");   
        }

    }

}
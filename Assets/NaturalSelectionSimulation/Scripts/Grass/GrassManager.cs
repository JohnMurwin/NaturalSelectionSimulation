using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class GrassManager : MonoBehaviour
    {
        #region Public Variabls

        

        #endregion
        

        #region PrivateVariables

        private GameObject[] grassObjects;


        #endregion


        #region UnityMethods

        private void Start()
        {
            if (grassObjects == null) // no grass objects found
                grassObjects = GameObject.FindGameObjectsWithTag("Grass");  // grabs all grass objects with the tag 'Grass'

            int count = 0;
            foreach (GameObject grassObject in grassObjects)
            {
                count++;
                //grassObject.Test();
            }
            
            Debug.Log("A total of: " + count + " grass objects...");
        }

        #endregion


        #region PrivateMethods



        #endregion

    }
}
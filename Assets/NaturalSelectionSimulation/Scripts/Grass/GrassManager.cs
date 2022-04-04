using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class GrassManager : MonoBehaviour
    {
        #region Public Variables

        

        #endregion
        

        #region Private Variables

        private GameObject[] grassObjects;


        #endregion


        #region Unity Methods

        private void Start()
        {
            if (grassObjects == null) // no grass objects found
                grassObjects = GameObject.FindGameObjectsWithTag("Grass");  // grabs all grass objects with the tag 'Grass'

            int count = 0;
            foreach (GameObject grassObject in grassObjects)
            {
                count++;
                
                /* THIS IS HOW YOU CALL A GAMEOBJECTS EMBEDDED SCRIPT
                Grass call = (Grass) grassObject.GetComponent(typeof(Grass));
                call.Test();
                */
            }
            
            Debug.Log("A total of: " + count + " grass objects...");
        }

        #endregion


        #region Private Methods



        #endregion

    }
}
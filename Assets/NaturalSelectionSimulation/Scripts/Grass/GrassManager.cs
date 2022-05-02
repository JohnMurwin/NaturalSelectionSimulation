using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class GrassManager : MonoBehaviour
    {
        #region Public Variables

        

        #endregion
        

        #region Private Variables

        private GameObject[] _grassObjects;


        #endregion


        #region Unity Methods

        private void Start()
        {
            if (_grassObjects == null) // no grass objects found
                _grassObjects = GameObject.FindGameObjectsWithTag("Grass");  // grabs all grass objects with the tag 'Grass'

            DebugGrassCount();
        }

        

        #endregion


        #region Private Methods
        
        /// <summary>
        /// 
        /// </summary>
        public GameObject FindNearestGrass(Vector3 pos)
        {
            float distance = Mathf.Infinity;
            GameObject selectedGrass = null;

            foreach (var grass in _grassObjects)   // check each hit collider
            {
                var grassObject = grass.transform.gameObject;

                if (grassObject != null && grassObject.GetComponent<Grass>().IsAlive)
                {
                    // check distance
                    var tempDistance = Vector3.Distance(grassObject.transform.position, pos);

                    // if closer
                    if (tempDistance <= distance)
                    {
                        distance = tempDistance;
                        selectedGrass = grassObject; // select this as closer
                    }
                }
            }

            return selectedGrass;   // my trarget
        }
        
        /// <summary>
        /// Helps to count all grass objects to ensure proper collection
        /// </summary>
        private void DebugGrassCount()
        {
            int count = 0;
            foreach (GameObject grassObject in _grassObjects)
            {
                Grass grass = (Grass) grassObject.GetComponent(typeof(Grass));

                if (grass.IsAlive)
                {
                    count++;
                }
            }

            Debug.Log("A total of: " + count + " grass objects...");

            //For testing purposes only, will kill plants after 5 seconds and then spawn them back 5 seconds later
            //StartCoroutine(TestPlants()); 
        }


        #endregion

    }
}
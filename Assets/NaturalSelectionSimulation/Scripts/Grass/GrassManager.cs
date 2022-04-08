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


        #region Private Methods

        //For testing purposes only
        private IEnumerator TestPlants()
        {
            //Fade in
            yield return TestKillPlants();

            //Wait for the duration
            float counter = 0;
            while (counter < 5f)
            {
                counter += Time.deltaTime;
                yield return null;
            }

            //Fade out
            yield return TestSpawnPlants();
        }

        //For testing purposes only
        private IEnumerator TestKillPlants()
        {
            //Wait for the duration
            float counter = 0;
            while (counter < 5f)
            {
                counter += Time.deltaTime;
                yield return null;
            }

            foreach (GameObject grassObject in grassObjects)
            {
                Grass grass = (Grass)grassObject.GetComponent(typeof(Grass));
                grass.KillPlant();
            }
        }

        //For testing purposes only
        private IEnumerator TestSpawnPlants()
        {
            //Wait for the duration
            float counter = 0;
            while (counter < 5f)
            {
                counter += Time.deltaTime;
                yield return null;
            }

            foreach (GameObject grassObject in grassObjects)
            {
                Grass grass = (Grass)grassObject.GetComponent(typeof(Grass));
                grass.SpawnPlant();
            }
        }

        #endregion

    }
}
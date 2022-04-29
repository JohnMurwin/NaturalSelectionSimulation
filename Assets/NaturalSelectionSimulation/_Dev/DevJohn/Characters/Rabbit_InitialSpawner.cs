using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NaturalSelectionSimulation
{
    public class Rabbit_InitialSpawner : MonoBehaviour
    {
        #region PublicVariables
        public int spawnCount = 100;
        public float spawnRate = 0.0f;
        public float spawnRange = 200f;
        
        public GameObject[] rabbitPrefabs;
        public GameObject rabbitParentContainer;

        public RabbitTraits_BaseSO RabbitTraitsBaseSo;
        
        #endregion

        #region PrivateVariables

        private Vector3 _spawnPos;


        #endregion
        

        #region UnityMethods

        private void Awake()
        {
            SpawnAnimals();
        }

        #endregion

        #region CustomMethods

        /// <summary>
        /// Iterates and spawns rabbitPrefabs spawnCount times
        /// </summary>
        public void SpawnAnimals()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                FindSpawnPosition();    // Find & Validate a spawn position
                
                GameObject obj = Instantiate(rabbitPrefabs[Random.Range(0,rabbitPrefabs.Length)], new Vector3(_spawnPos.x, 0.5f, _spawnPos.z), Quaternion.identity);  // instantiate rabbit prefab at our spawn pos 
                
                obj.transform.parent = rabbitParentContainer.transform; // set spawned rabbits parent to parent container

                SetInitialGenes(obj, i);   // sets all initial genes
            }
            
            Debug.Log("Last Rabbit Spawned... a total of: " + spawnCount + " spawned.");
        }

        /// <summary>
        /// Sets Initial Rabbits Genes based off RabbitTraits_BaseSO & functions
        /// </summary>
        /// <param name="rabbit"></param>
        private void SetInitialGenes(GameObject rabbit, int spawnNumber)
        {
            // Health
            rabbit.GetComponent<Rabbit_Genes>().Health = RabbitTraitsBaseSo.Health();

            // Stamina
            rabbit.GetComponent<Rabbit_Genes>().Stamina = RabbitTraitsBaseSo.Stamina();

            // Size
            float size = RabbitTraitsBaseSo.Size();
            rabbit.GetComponent<Rabbit_Genes>().Size = size;
            rabbit.transform.localScale = new Vector3(size, size, size);

            // Speed
            rabbit.GetComponent<Rabbit_Genes>().Speed = RabbitTraitsBaseSo.Speed();
            
            // Sensory Distance
            rabbit.GetComponent<Rabbit_Genes>().SensoryDistance = RabbitTraitsBaseSo.SensoryDistance();
            
            // Gender
            if (spawnNumber % 2 == 0)
            {
                rabbit.GetComponent<Rabbit_Genes>().Gender = Rabbit_Genes.Genders.Female; // even == male
            }
            else
            {
                rabbit.GetComponent<Rabbit_Genes>().Gender = Rabbit_Genes.Genders.Male;   // odd == female
            }
            
            // Gestation (only females get)
            rabbit.GetComponent<Rabbit_Genes>().GestationDuration = RabbitTraitsBaseSo.GestationDuration();

            // Growth
            rabbit.GetComponent<Rabbit_Genes>().GrowthTime = RabbitTraitsBaseSo.GrowthTime();

            // Desirability (females will use this number as a target)
            rabbit.GetComponent<Rabbit_Genes>().Desirability = RabbitTraitsBaseSo.Desirability();

            // Reproductive Urge
            rabbit.GetComponent<Rabbit_Genes>().ReproductiveUrge = RabbitTraitsBaseSo.ReproductiveUrge();
        }


        /// <summary>
        /// Randomly selects a position inside a Unit Sphere with radius spawnRange
        /// </summary>
        void FindSpawnPosition()
        {
            // find random position inside UnitSphere of distance _wanderRange
            var randomPoint = transform.position + Random.insideUnitSphere * spawnRange;

            // set tempTarget position to that position
            var targetPos = randomPoint;

            // see if position is valid
            ValidatePosition(ref targetPos);
            
            // set nav target to position
            _spawnPos = targetPos;
        }
        
        /// <summary>
        /// Validate that random Nav position is a valid position
        /// </summary>
        /// <param name="targetPosition">Position to check</param>
        void ValidatePosition(ref Vector3 targetPosition)
        {

                UnityEngine.AI.NavMeshHit hit; // point for good hit

                if (!UnityEngine.AI.NavMesh.SamplePosition(targetPosition, out hit, Mathf.Infinity,
                        1 << UnityEngine.AI.NavMesh.GetAreaFromName("Walkable")))
                {
                    Debug.LogError("Unable to get NavMesh hit sample. Need a Nav Mesh layer with name 'Walkable'");
                    enabled = false;
                    return;
                }

                targetPosition = hit.position;  // if we find a good location, return it
        }

        #endregion
    }
}

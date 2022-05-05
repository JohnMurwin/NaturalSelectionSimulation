using UnityEngine;
using Random = UnityEngine.Random;

namespace NaturalSelectionSimulation
{
    
    
    public class Rabbit_ReproductionController : MonoBehaviour
    {
        #region Public Variables

        public GameObject[] rabbitPrefabs;
        public GameObject rabbitParentContainer;

        #endregion

        #region Private Variables
        

        

        #endregion

        #region Unity Methods

        

        #endregion

        #region Custom Methods

        public void SpawnRabbit(Vector3 spawnPosition, GameObject father, GameObject mother)
        {
            // Instantiate GameObject
            GameObject obj = Instantiate(rabbitPrefabs[Random.Range(0,rabbitPrefabs.Length)], new Vector3(spawnPosition.x, 0.5f, spawnPosition.z), Quaternion.identity);  // instantiate rabbit prefab at 
        
            // make baby small
            float initialSize = Random.Range(0f, 0.3f);
            obj.transform.localScale = new Vector3(initialSize, initialSize, initialSize);
                
            // set spawned rabbits parent to parent container
            obj.transform.parent = rabbitParentContainer.transform; 
            
            // Gene Setting
            SetGenes(obj, father, mother);
        }

        private static void SetGenes(GameObject obj, GameObject father, GameObject mother )
        {
            obj.GetComponent<Rabbit_Genes>().GenerateNewGeneCollection(father.GetComponent<Rabbit_Genes>(),mother.GetComponent<Rabbit_Genes>());
        }

        #endregion
        
        

    }
}
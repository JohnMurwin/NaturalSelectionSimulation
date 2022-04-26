using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class Rabbit_MateController : MonoBehaviour
    {
        #region Public Variables

        private GameObject[] rabbitList;


        #endregion

        #region Private Variables



        #endregion

        #region Unity Methods



        #endregion

        #region Custom Methods

        public static GameObject FindMate(Vector3 pos, float radius)
        {
            GameObject selectedMate = null;

            Collider[] hitColliders = Physics.OverlapSphere(pos,radius);

            var debugCount = 0;

            if (hitColliders != null)
            {
                foreach (var hit in hitColliders)
                {
                    if (hit.CompareTag("Rabbit"))
                    {
                        var rabbitObject = hit.transform.gameObject;

                        if (rabbitObject.GetComponent<Rabbit_Genes>().Gender == Rabbit_Genes.Genders.Female)
                        {
                            debugCount++;
                        
                            selectedMate = rabbitObject;    // TODO: convert this to a list of 'potential' mates and then find closest
                        }
                    }
                }
            }

            Debug.Log(debugCount + " potential mates found");
            
            return selectedMate;
        }



        #endregion
    }
}
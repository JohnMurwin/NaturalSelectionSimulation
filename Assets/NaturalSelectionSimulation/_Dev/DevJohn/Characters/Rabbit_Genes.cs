using UnityEngine;
using Random = UnityEngine.Random;

namespace NaturalSelectionSimulation
{
    public class Rabbit_Genes : MonoBehaviour
    {
        #region Private Variables
        
        private const float MutationChanceDecider = 0.75f;

        #endregion
        
        #region Public Variables
        public enum Genders
        {
            Male,
            Female
        }

        public Genders Gender { get; set; }
        public bool IsChild { get; set; }
        public float Health { get; set; }
        public float Stamina { get; set; }
        public float Size { get; set; }
        public float Speed { get; set; }
        public float SensoryDistance { get; set; }
        public float GestationDuration { get; set; }
        public float LifeExpectancy { get; set; }
        public float GrowthTime { get; set; }
        public float Desirability { get; set; }
        public float ReproductiveUrge { get; set; }
        

        #endregion
        
        
        #region Unity Methods

        #endregion

        #region Custom Functions
        
        /// <summary>
        /// Triggers the 'replication' of genes based off father and mother objects. Sets the genes on the rabbit object calling this function.
        /// </summary>
        /// <param name="father">rabbit Genes object serving as father for gene replication</param>
        /// <param name="mother">rabbit Genes object serving as mother for gene replication</param>
        public void GenerateNewGeneCollection(Rabbit_Genes father, Rabbit_Genes mother)
        {
            Health = ReproduceGene(father.Health, mother.Health);
            Stamina = ReproduceGene(father.Stamina, mother.Stamina);
            Speed = ReproduceGene(father.Speed, mother.Speed);
            Size = ReproduceGene(father.Size, mother.Size);
            SensoryDistance = ReproduceGene(father.SensoryDistance, mother.SensoryDistance);
            LifeExpectancy = ReproduceGene(father.LifeExpectancy, mother.LifeExpectancy);
            GestationDuration = ReproduceGene(father.ReproductiveUrge, mother.ReproductiveUrge);
            ReproductiveUrge = ReproduceGene(father.ReproductiveUrge, mother.ReproductiveUrge);
            GrowthTime = ReproduceGene(father.GrowthTime, mother.GrowthTime);
            Desirability =  ReproduceGene(father.Desirability, mother.Desirability);
            Gender = ReproduceGender();
            IsChild = true;
        }

        
        /// <summary>
        /// Uses a bit of random math to calculate the genes to return (either using father/mother values, or mutating them)
        /// </summary>
        /// <param name="father">rabbit Gene float serving as father for gene replication</param>
        /// <param name="mother">rabbit Gene float serving as mother for gene replication</param>
        /// <returns>calculated gene value</returns>
        private static float ReproduceGene(float father, float mother)
        {
            float returnGene;
            float parentChance = Random.Range(0f, 1f);
            float mutationChance = Random.Range(0f, 1f);

            if (parentChance < 0.5f)
            {
                if (mutationChance >= MutationChanceDecider)
                    returnGene = father + (father * Random.Range(-1f, 1f));  // fathers gene + mutation
                else
                    returnGene = father;    // fathers gene
            }
            else
            {
                if (mutationChance >= MutationChanceDecider)
                    returnGene = mother + (mother * Random.Range(-1f, 1f));  // mothers gene + mutation
                else
                    returnGene = mother;
            }

            return returnGene;
        }
        
        /// <summary>
        /// As Gender is an enum, need to calculate seperately, but similiar in theory to ReproduceGene without mutation
        /// </summary>
        /// <returns>Genders type</returns>
        private Genders ReproduceGender()
        {
            float parentChance = Random.Range(0f, 1f);

            if (parentChance < 0.5f)
                return Genders.Male;
            
            
            return Genders.Female;
            
        }

        #endregion
        
    }
}
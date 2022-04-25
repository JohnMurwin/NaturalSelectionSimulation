using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NaturalSelectionSimulation
{
    public class Rabbit_Genes : MonoBehaviour
    {
        #region Public Variables
        public enum Genders
        {
            Male,
            Female
        }

        //! DEBUG
        public Genders gender;
        public float health;
        public float stamina;
        public float size;
        public float speed;
        public float sensoryDistance;
        public float gestationDuration;
        public float growthTime;
        public float desirability;
        public float reproductiveUrge;
        //! / DEBUG

        public Genders Gender { get; set; }
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
        
        #region Private Variables
        
        public float _mutationChanceDecider = 0.75f;

        #endregion


        #region Public Functions
        
        

        
        public void GenerateNewGeneCollection(Rabbit_Genes father, Rabbit_Genes mother)
        {

            float parentChance = Random.Range(0f, 1f);
            float mutationChance = Random.Range(0f, 1f);
            
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Health = father.Health + (father.Health * Random.Range(0f, 1f));
                }
                else
                {
                    Health = father.Health;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Health = mother.Health + (mother.Health * Random.Range(0f, 1f));
                }
                else
                {
                    Health = mother.Health;
                }
            }
            
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Stamina = father.Stamina + (father.Stamina * Random.Range(0f, 1f));
                }
                else
                {
                    Stamina = father.Stamina;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Stamina = mother.Stamina + (mother.Stamina * Random.Range(0f, 1f));
                }
                else
                {
                    Stamina = mother.Stamina;
                }
            }
            
            
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Speed = father.Speed + (father.Speed * Random.Range(0f, 1f));
                }
                else
                {
                    Speed = father.Speed;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Speed = mother.Speed + (mother.Speed * Random.Range(0f, 1f));
                }
                else
                {
                    Speed = mother.Speed;
                }
            }

            parentChance = Random.Range(0f, 1f);
            mutationChance = Random.Range(0f, 1f);
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Size = father.Size + (father.Size * Random.Range(0f, 1f));
                }
                else
                {
                    Size = father.Size;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Size = mother.Size + (mother.Size * Random.Range(0f, 1f));
                }
                else
                {
                    Size = mother.Size;
                }
            }

            parentChance = Random.Range(0f, 1f);
            mutationChance = Random.Range(0f, 1f);
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    SensoryDistance =
                        father.SensoryDistance + (father.SensoryDistance * Random.Range(0f, 1f));
                }
                else
                {
                    SensoryDistance = father.SensoryDistance;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    SensoryDistance =
                        mother.SensoryDistance + (mother.SensoryDistance * Random.Range(0f, 1f));
                }
                else
                {
                    SensoryDistance = mother.SensoryDistance;
                }
            }


            parentChance = Random.Range(0f, 1f);
            mutationChance = Random.Range(0f, 1f);
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    LifeExpectancy = father.LifeExpectancy + (father.LifeExpectancy * Random.Range(0f, 1f));
                }
                else
                {
                    LifeExpectancy = father.LifeExpectancy;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    LifeExpectancy = mother.LifeExpectancy + (mother.LifeExpectancy * Random.Range(0f, 1f));
                }
                else
                {
                    LifeExpectancy = mother.LifeExpectancy;
                }
            }

            parentChance = Random.Range(0f, 1f);
            mutationChance = Random.Range(0f, 1f);
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    GestationDuration = father.GestationDuration + (father.GestationDuration * Random.Range(0f, 1f));
                }
                else
                {
                    GestationDuration = father.GestationDuration;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    GestationDuration = mother.GestationDuration + (mother.GestationDuration * Random.Range(0f, 1f));
                }
                else
                {
                    GestationDuration = mother.GestationDuration;
                }
            }

            parentChance = Random.Range(0f, 1f);
            mutationChance = Random.Range(0f, 1f);
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    ReproductiveUrge =
                        father.ReproductiveUrge + (father.ReproductiveUrge * Random.Range(0f, 1f));
                }
                else
                {
                    ReproductiveUrge = father.ReproductiveUrge;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    ReproductiveUrge =
                        mother.ReproductiveUrge + (mother.ReproductiveUrge * Random.Range(0f, 1f));
                }
                else
                {
                    ReproductiveUrge = mother.ReproductiveUrge;
                }
            }

            parentChance = Random.Range(0f, 1f);
            mutationChance = Random.Range(0f, 1f);
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    GrowthTime = father.GrowthTime + (father.GrowthTime * Random.Range(0f, 1f));
                }
                else
                {
                    GrowthTime = father.GrowthTime;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    GrowthTime = mother.GrowthTime + (mother.GrowthTime * Random.Range(0f, 1f));
                }
                else
                {
                    GrowthTime = mother.GrowthTime;
                }
            }

            parentChance = Random.Range(0f, 1f);
            mutationChance = Random.Range(0f, 1f);
            if (parentChance < 0.5f)
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Desirability = father.Desirability + (father.Desirability * Random.Range(0f, 1f));
                }
                else
                {
                    Desirability = father.Desirability;
                }
            }
            else
            {
                if (mutationChance >= _mutationChanceDecider)
                {
                    Desirability = mother.Desirability + (mother.Desirability * Random.Range(0f, 1f));
                }
                else
                {
                    Desirability = mother.Desirability;
                }
            }

            parentChance = Random.Range(0f, 1f);
            if (parentChance < 0.5f)
            {
                Gender = Genders.Male;
            }
            else
            {
                Gender = Genders.Female;
            }
        }
        

        #endregion

        #region Unity Methods

        private void Start()
        {
            //TODO: Remove debug components for Gene display
            gender = Gender;
            health = Health;
            stamina = Stamina;
            size = Size;
            speed = Speed;
            sensoryDistance = SensoryDistance;
            gestationDuration = GestationDuration;
            growthTime = GrowthTime;
            desirability = Desirability;
            reproductiveUrge = ReproductiveUrge;
        }

        #endregion
        
    }
}
using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class Rabbit_Genes : MonoBehaviour
    {
        public enum Genders
        {
            Male,
            Female
        }
        
        #region Private Variables

        
        

        #endregion
        #region Public Variables

        //! DEBUG
        public Genders gender;
        public float health;
        public float stamina;
        public float hunger;
        public float thirst;
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
        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Size { get; set; }
        public float Speed { get; set; }
        public float SensoryDistance { get; set; }
        public float GestationDuration { get; set; }
        public float GrowthTime { get; set; }
        public float Desirability { get; set; }
        public float ReproductiveUrge { get; set; }

        #endregion


        #region Public Functions
        

        #endregion

        #region Unity Methods

        private void Start()
        {
            //TODO: Remove debug components for Gene display
            gender = Gender;
            health = Health;
            stamina = Stamina;
            hunger = Hunger;
            thirst = Thirst;
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
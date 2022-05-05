using System;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NaturalSelectionSimulation
{
    [CreateAssetMenu(fileName = "New Rabbit Stats", menuName = "NSS/New Rabbit Stats", order = 0)]
    public class RabbitTraits_BaseSO : ScriptableObject
    {
        #region Public Traits
        [SerializeField, Tooltip("How much health this animal has.")]
        public float lifeExpectancy = 360f;

        [SerializeField, Tooltip("How much health this animal has.")]
        public float health = 100f;

        [SerializeField, Tooltip("How many seconds this animal can run for before it gets tired.")]
        public float stamina = 10f;

        [SerializeField, Tooltip("How big (or small) this animal is.")]
        public float size = 2f;

        [SerializeField, Tooltip("How fast this animal is.")]
        public float speed = 2f;

        [SerializeField, Tooltip("How far this animal can hear other animals.")]
        public float hearingDistance = 40f;

        [SerializeField, Tooltip("How far this animal can see other objects and animals.")]
        public float sightDistance = 20f;

        [SerializeField, Tooltip("How long this animal takes to grow its offspring until viable birth.")]
        public float gestationDuration = 60f;

        [SerializeField, Tooltip("How fast this animal grows to be an adult.")]
        public float growthTime = 120f;

        [SerializeField, Tooltip("How desirable this animal is to its mates (females will use this number as a target)).")]
        public float desirability = 100f;

        [SerializeField, Tooltip("How quickly this animal will want to mate again (if pregnant, disabled until offspring birthed).")]
        public float reproductiveUrge = 150f;

        #endregion


        #region TraitFunctions

        public float LifeExpectancy()
        {
            return ValueRandomizerFormula(lifeExpectancy, 1f);
        }

        public float Health()
        {
            return ValueRandomizerFormula(health, 1f);
        }

        public float Stamina()
        {
            return ValueRandomizerFormula(stamina, 1f);
        }

        public float Size()
        {
            return ValueRandomizerFormula(size, 1f, 0.75f);
        }

        public float Speed()
        {
            return ValueRandomizerFormula(speed, 1f);
        }

        public float SensoryDistance()
        {
            float hearingWeight = 0.8f; // how important is hearing compared to sight
            float sightWeight = 0.2f; // how important is sight compare to hearing

            return (ValueRandomizerFormula(hearingDistance, hearingWeight) +
                    ValueRandomizerFormula(sightDistance, sightWeight));
        }

        public float GestationDuration()
        {
            return ValueRandomizerFormula(gestationDuration, 1f);
        }

        public float GrowthTime()
        {
            return ValueRandomizerFormula(growthTime, 1f);
        }

        public float Desirability()
        {
            return ValueRandomizerFormula(desirability, 1f);
        }

        public float ReproductiveUrge()
        {
            return ValueRandomizerFormula(reproductiveUrge, 1f);
        }


        /// <summary>
        /// ValueRandomizerFormula: Takes in a value and a weight and randomly returns a value based off RandomRange and Weight
        /// </summary>
        /// <param name="primaryValue">Value to be randomized</param>
        /// <param name="weight">how important that value is, 1 for only 1 trait</param>
        /// <returns>Weighted Random Value</returns>
        private float ValueRandomizerFormula(float primaryValue, float weight, float floorValue = 0.1f)
        {
            float value = Mathf.Round((primaryValue * Random.Range(floorValue, 1f) * weight) * 100.0f) *
                          0.01f; // will round to 2 decimal places
            return value;
        }

        #endregion
    }
}
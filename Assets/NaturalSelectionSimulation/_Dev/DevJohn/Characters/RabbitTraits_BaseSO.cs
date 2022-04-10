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
        [SerializeField, Tooltip("How fast this animal is.")]
            public float speed = 1f;

            [SerializeField, Tooltip("How many seconds this animal can run for before it gets tired.")]
            public float stamina = 10f;

            [SerializeField, Tooltip("How much health this animal has.")]
            public float health = 100f;
            
            [SerializeField, Tooltip("How big (or small) this animal is.")]
            public float size = 1f;
            
            [SerializeField, Tooltip("How far this animal can hear other animals.")]
            public static float hearingDistance = 40f;
            
            [SerializeField, Tooltip("How far this animal can see other objects and animals.")]
            public static float sightDistance = 20f;
            
            [SerializeField, Tooltip("How long this animal takes to grow its offspring until viable birth.")]
            public float gestationDuration = 15f;
            
            [SerializeField, Tooltip("How fast this animal is.")]
            public float growthTime = 4f;
            
            [SerializeField, Tooltip("How desirable this animal is to its mates (male only).")]
            public float desirability = 1f;
            
            [SerializeField, Tooltip("How quickly this animal will want to mate again (if pregnant, disabled until offspring birthed).")]
            public float reproductiveUrge = 1f;
            
        #endregion


        #region TraitFunctions

        /// <summary>
        /// Sensory distance seeder.
        /// </summary>
        /// <returns>Rabbits total sensory distance as a function of Hearing & Sight</returns>
        public static float SensoryDistance()
        {
            float hearingWeight = 0.8f; // how important is hearing compared to sight
            float sightWeight = 0.2f;   // how important is sight compare to hearing

            return (ValueRandomizerFormula(hearingDistance, hearingWeight) + ValueRandomizerFormula(sightDistance, sightWeight));
        }

        /// <summary>
        /// ValueRandomizerFormula: Takes in a value and a weight and randomly returns a value based off RandomRange and Weight
        /// </summary>
        /// <param name="primaryValue">Value to be randomized</param>
        /// <param name="weight">how important that value is, 1 for only 1 trait</param>
        /// <returns>Weighted Random Value</returns>
        private static float ValueRandomizerFormula(float primaryValue, float weight)
        {
            float value = Mathf.Round((primaryValue * Random.Range(0.1f, 1f) * weight) * 100.0f) * 0.01f;   // will round to 2 decimal places
            return value;
        }

        #endregion
    }
}
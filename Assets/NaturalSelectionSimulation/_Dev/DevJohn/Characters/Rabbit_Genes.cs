using System;
using UnityEngine;

namespace NaturalSelectionSimulation
{
    public class Rabbit_Genes : MonoBehaviour
    {
        #region Public Variables
        
        [SerializeField] public enum Genders
        {
            Male,
            Female
        }
        [SerializeField] public Genders Gender { get; set; }
        [SerializeField] public float SensoryDistance { get; set; }
        


        #endregion
    }
}
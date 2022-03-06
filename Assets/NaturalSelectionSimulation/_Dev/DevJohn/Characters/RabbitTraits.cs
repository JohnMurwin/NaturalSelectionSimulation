using UnityEngine;

namespace NaturalSelectionSimulaiton
{
    [CreateAssetMenu(fileName = "New Rabbit Stats", menuName = "NSS/New Rabbit Stats", order = 0)]
    public class RabbitTraits : ScriptableObject
    {
        [SerializeField, Tooltip("How fast this animal is.")]
        public float speed = 1f;

        [SerializeField, Tooltip("How many seconds this animal can run for before it gets tired.")]
        public float stamina = 10f;

        [SerializeField, Tooltip("How much health this animal has.")]
        public float health = 100f;
        
        [SerializeField, Tooltip("How big (or small) this animal is.")]
        public float size = 1f;
        
        [SerializeField, Tooltip("How far this animal can hear other animals.")]
        public float hearingDistance = 20f;
        
        [SerializeField, Tooltip("How far this animal can see other objects and animals.")]
        public float sightDistance = 15f;
        
        [SerializeField, Tooltip("How long this animal takes to grow its offspring until viable birth.")]
        public float gestationDuration = 15f;
        
        [SerializeField, Tooltip("How fast this animal is.")]
        public float growthTime = 4f;
        
        [SerializeField, Tooltip("How desirable this animal is to its mates (male only).")]
        public float desirability = 1f;
        
        [SerializeField, Tooltip("How quickly this animal will want to mate again (if pregnant, disabled until offspring birthed).")]
        public float reproductiveUrge = 1f;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitGenesScript : MonoBehaviour
{
    public class RabbitGeneCollection
    {
        private float speed;
        private float size;
        private float hearingDistance;
        private float sightDistance;
        private float lifeExpectancy;
        private float gestation;
        private float reproductiveUrge;
        private float growthTime;
        private float desirability;
        private bool isMale;

        public RabbitGeneCollection(float spd, float sz, float hDistance, float sDistance, float lExpectancy,
                                    float g, float rUrge, float gTime, float d, bool male)
        {
            speed = spd;
            size = sz;
            hearingDistance = hDistance;
            sightDistance = sDistance;
            lifeExpectancy = lExpectancy;
            gestation = g;
            reproductiveUrge = rUrge;
            growthTime = gTime;
            desirability = d;
            isMale = male;
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public float HearingDistance
        {
            get { return hearingDistance; }
            set { hearingDistance = value; }
        }

        public float SightDistance
        {
            get { return sightDistance; }
            set { sightDistance = value; }
        }

        public float LifeExpectancy
        {
            get { return lifeExpectancy; }
            set { lifeExpectancy = value; }
        }

        public float Gestation
        {
            get { return gestation; }
            set { gestation = value; }
        }

        public float ReproductiveUrge
        {
            get { return reproductiveUrge; }
            set { reproductiveUrge = value; }
        }

        public float GrowthTime
        {
            get { return growthTime; }
            set { growthTime = value; }
        }

        public float Desirability
        {
            get { return desirability; }
            set { desirability = value; }
        }

        public bool IsMale
        {
            get { return isMale; }
            set { isMale = value; }
        }
    }
}

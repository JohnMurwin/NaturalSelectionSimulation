using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitGeneManager : MonoBehaviour
{
    public float _mutationChanceDecider = 0.75f;

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

        public RabbitGeneCollection()
        {
            speed = 0;
            size = 0;
            hearingDistance = 0;
            sightDistance = 0;
            lifeExpectancy = 0;
            gestation = 0;
            reproductiveUrge = 0;
            growthTime = 0;
            desirability = 0;
            isMale = true;
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

    RabbitGeneCollection GenerateNewGeneCollection(RabbitGeneCollection rabbit1, RabbitGeneCollection rabbit2)
    {
        RabbitGeneCollection newRabbit = new RabbitGeneCollection();
        float parentChance = Random.Range(0f, 1f);
        float _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.Speed = rabbit1.Speed + (rabbit1.Speed * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.Speed = rabbit1.Speed;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.Speed = rabbit2.Speed + (rabbit2.Speed * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.Speed = rabbit2.Speed;
            }
        }

        parentChance = Random.Range(0f, 1f);
        _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.Size = rabbit1.Size + (rabbit1.Size * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.Size = rabbit1.Size;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.Size = rabbit2.Size + (rabbit2.Size * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.Size = rabbit2.Size;
            }
        }

        parentChance = Random.Range(0f, 1f);
        _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.HearingDistance = rabbit1.HearingDistance + (rabbit1.HearingDistance * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.HearingDistance = rabbit1.HearingDistance;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.HearingDistance = rabbit2.HearingDistance + (rabbit2.HearingDistance * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.HearingDistance = rabbit2.HearingDistance;
            }
        }

        parentChance = Random.Range(0f, 1f);
        _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.SightDistance = rabbit1.SightDistance + (rabbit1.SightDistance * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.SightDistance = rabbit1.SightDistance;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.SightDistance = rabbit2.SightDistance + (rabbit2.SightDistance * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.SightDistance = rabbit2.SightDistance;
            }
        }

        parentChance = Random.Range(0f, 1f);
        _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.LifeExpectancy = rabbit1.LifeExpectancy + (rabbit1.LifeExpectancy * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.LifeExpectancy = rabbit1.LifeExpectancy;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.LifeExpectancy = rabbit2.LifeExpectancy + (rabbit2.LifeExpectancy * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.LifeExpectancy = rabbit2.LifeExpectancy;
            }
        }

        parentChance = Random.Range(0f, 1f);
        _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.Gestation = rabbit1.Gestation + (rabbit1.Gestation * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.Gestation = rabbit1.Gestation;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.Gestation = rabbit2.Gestation + (rabbit2.Gestation * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.Gestation = rabbit2.Gestation;
            }
        }

        parentChance = Random.Range(0f, 1f);
        _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.ReproductiveUrge = rabbit1.ReproductiveUrge + (rabbit1.ReproductiveUrge * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.ReproductiveUrge = rabbit1.ReproductiveUrge;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.ReproductiveUrge = rabbit2.ReproductiveUrge + (rabbit2.ReproductiveUrge * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.ReproductiveUrge = rabbit2.ReproductiveUrge;
            }
        }

        parentChance = Random.Range(0f, 1f);
        _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.GrowthTime = rabbit1.GrowthTime + (rabbit1.GrowthTime * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.GrowthTime = rabbit1.GrowthTime;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.GrowthTime = rabbit2.GrowthTime + (rabbit2.GrowthTime * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.GrowthTime = rabbit2.GrowthTime;
            }
        }

        parentChance = Random.Range(0f, 1f);
        _mutationChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.Desirability = rabbit1.Desirability + (rabbit1.Desirability * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.Desirability = rabbit1.Desirability;
            }
        }
        else
        {
            if (_mutationChance >= _mutationChanceDecider)
            {
                newRabbit.Desirability = rabbit2.Desirability + (rabbit2.Desirability * Random.Range(0f, 1f));
            }
            else
            {
                newRabbit.Desirability = rabbit2.Desirability;
            }
        }

        parentChance = Random.Range(0f, 1f);
        if (parentChance < 0.5f)
        {
            newRabbit.IsMale = true;
        }
        else
        {
            newRabbit.IsMale = false;
        }
        return newRabbit;
    }

    void Start()
    {
        //testing
        //make rabbit 1
        var maleRabbitGenes = new RabbitGeneCollection(7, 8, 8, 5, 4, 5, 8, 5, 6, true);
        PrintAttributes(maleRabbitGenes);
        //make rabbit 2
        var femaleRabbitGenes = new RabbitGeneCollection(6, 7, 9, 6, 3, 6, 5, 7, 4, false);
        PrintAttributes(femaleRabbitGenes);

        //lets make 5 offsrping and see the results
        for (int i = 0; i < 5; i++)
        {
            var rabbitOffspringGenes = GenerateNewGeneCollection(maleRabbitGenes, femaleRabbitGenes);
            PrintAttributes(rabbitOffspringGenes);
        }
    }

    int rabbitCount = 1;
    void PrintAttributes(RabbitGeneCollection rabbitGenes)
    {
        Debug.Log("Rabbit: " + rabbitCount.ToString());
        Debug.Log("Speed: " + rabbitGenes.Speed.ToString());
        Debug.Log("Size: " + rabbitGenes.Size.ToString());
        Debug.Log("Sight Distance: " + rabbitGenes.SightDistance.ToString());
        Debug.Log("Reproductive Urge: " + rabbitGenes.ReproductiveUrge.ToString());
        Debug.Log("Life Expectancy: " + rabbitGenes.LifeExpectancy.ToString());
        Debug.Log("Hearing Distance: " + rabbitGenes.HearingDistance.ToString());
        Debug.Log("Growth Time: " + rabbitGenes.GrowthTime.ToString());
        Debug.Log("Gestation: " + rabbitGenes.Gestation.ToString());
        Debug.Log("Desirability: " + rabbitGenes.Desirability.ToString());
        Debug.Log("Is Male: " + rabbitGenes.IsMale.ToString());
        Debug.Log("");
        rabbitCount++;
    }
}
